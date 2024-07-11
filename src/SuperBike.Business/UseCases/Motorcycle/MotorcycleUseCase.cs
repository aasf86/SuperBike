using Microsoft.Extensions.Logging;
using SuperBike.Auth.Config;
using SuperBike.Business.Dtos.Motorcycle;
using SuperBike.Business.Dtos.Motorcycle.Request;
using SuperBike.Business.Dtos.Motorcycle.Response;
using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;
using SuperBike.Business.UseCases.User;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using SuperBike.Domain.Contracts.UseCases;
using SuperBike.Domain.Contracts.UseCases.Motorcycle;
using SuperBike.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.UseCases.Motorcycle
{
    internal class MotorcycleUseCase : UseCaseBase, 
        IMotorcycleUseCase
        /*<
            MotorcycleInsertRequest, MotorcycleInsertResponse
        >*/
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private IMotorcycleRepository MotorcycleRepository => _motorcycleRepository;
        public MotorcycleUseCase(ILogger<MotorcycleUseCase> logger, IMotorcycleRepository motorcycleRepository) : base(logger) 
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<MotorcycleInsertResponse> Insert(MotorcycleInsertRequest motorcycleInsertRequest)
        {
            try
            {
                "Inciando [insert] de motocicleta: {Plate}".LogInf(motorcycleInsertRequest.Data.Plate);

                var motocycle = motorcycleInsertRequest.Data;
                var motorcycleInsertResponse = new MotorcycleInsertResponse(motocycle);
                var result = Validate(motocycle);

                if (!result.IsSuccess)
                {
                    motorcycleInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", motorcycleInsertResponse.Errors.ToArray());
                    $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(motocycle.Plate);                    
                    return motorcycleInsertResponse;
                }

                //aasf86 verificar se já existe placa
                //'MotorcycleRepository.GetAll' ou 'MotorcycleRepository.Get'

                await MotorcycleRepository.Insert(new Entity.Motorcycle(motocycle.Year, motocycle.Model, motocycle.Plate));

                return motorcycleInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [inserir] motocicleta: {Plate}".LogErr(motorcycleInsertRequest.Data.Plate);
                exc.Message.LogErr(exc);

                var motorcycleInsertResponse = new MotorcycleInsertResponse(motorcycleInsertRequest.Data);
                motorcycleInsertResponse.Errors.Add(exc.Message);
                return motorcycleInsertResponse;
            }            
        }
    }
}
