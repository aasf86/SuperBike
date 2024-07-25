using Microsoft.Extensions.Logging;
using Moq;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Motorcycle;
using SuperBike.Business.UseCases;
using SuperBike.Business.UseCases.Motorcycle;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using SuperBike.Domain.Contracts.Services;
using SuperBike.Domain.Entities;
using SuperBike.Infrastructure.Repositories.Motorcycle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Test
{
    public class UnitTestMotorcycleUseCase
    {
        private readonly MotorcycleUseCase _motorcycleUseCase;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepository;
        private readonly Mock<IMessageBroker> _messageBroker;
        private readonly Mock<ILogger<MotorcycleUseCase>> _logger;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly MotorcycleInsert _motorcycleInsert;
        private readonly Mock<IDbTransaction> _dbTransaction;

        public UnitTestMotorcycleUseCase()
        {
            _motorcycleRepository = new Mock<IMotorcycleRepository>();
            _messageBroker = new Mock<IMessageBroker>();
            _logger = new Mock<ILogger<MotorcycleUseCase>>();
            _dbConnection = new Mock<IDbConnection>();
            _dbTransaction = new Mock<IDbTransaction>();
            _motorcycleUseCase = new MotorcycleUseCase(
                _logger.Object,
                _motorcycleRepository.Object,
                _dbConnection.Object,
                _messageBroker.Object);

            _motorcycleInsert = new MotorcycleInsert 
            { 
                Model = "Super motoca",
                Plate = "ABC2345",
                Year = 2024
            };

            _dbConnection
                .Setup(x => x.BeginTransaction())
                .Returns(_dbTransaction.Object);
        }

        [Fact]
        public async void Insert_One_Motorcycle_With_Success()
        {
            var motorcycleInsertRequest = RequestBase.New(_motorcycleInsert);
            var response = await _motorcycleUseCase.Insert(motorcycleInsertRequest);

            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async void Insert_One_Motorcycle_With_Error()
        {
            _motorcycleInsert.Plate += "*";
            var motorcycleInsertRequest = RequestBase.New(_motorcycleInsert);
            var response = await _motorcycleUseCase.Insert(motorcycleInsertRequest);

            Assert.True(response.IsSuccess == false);
        }
    }
}
