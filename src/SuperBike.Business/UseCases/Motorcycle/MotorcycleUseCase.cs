using Microsoft.Extensions.Logging;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos.Motorcycle.Request;
using SuperBike.Business.Dtos.Motorcycle.Response;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using SuperBike.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using static SuperBike.Domain.Entities.Motorcycle;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.UseCases.Motorcycle
{
    public class MotorcycleUseCase : UseCaseBase, IMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private IMotorcycleRepository MotorcycleRepository => _motorcycleRepository;
        public MotorcycleUseCase(
            ILogger<MotorcycleUseCase> logger, 
            IMotorcycleRepository motorcycleRepository,
            IDbConnection dbConnection) : base(logger, dbConnection) 
        {
            _motorcycleRepository = motorcycleRepository;
            TransactionAssigner.Add(_motorcycleRepository.SetTransaction);
        }

        public async Task<MotorcycleInsertResponse> Insert(MotorcycleInsertRequest motorcycleInsertRequest)
        {
            try
            {
                "Inciando [insert] de motocicleta: {Plate}".LogInf(motorcycleInsertRequest.Data.Plate);

                var motocycle = motorcycleInsertRequest.Data;
                var motorcycleInsertResponse = new MotorcycleInsertResponse(motocycle) { RequestId = motorcycleInsertRequest.RequestId };
                var result = Validate(motocycle);

                if (!result.IsSuccess)
                {
                    motorcycleInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", motorcycleInsertResponse.Errors.ToArray());
                    $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(motocycle.Plate);                    
                    return motorcycleInsertResponse;
                }

                await UnitOfWorkExecute(async () =>
                {                    
                    var motocycleFromDb = await MotorcycleRepository.GetByPlate(motocycle.Plate);

                    if (motocycleFromDb is null)
                    {
                        await MotorcycleRepository.Insert(new Entity.Motorcycle(motocycle.Year, motocycle.Model, motocycle.Plate));
                    }
                    else
                    {
                        var strMsg = string.Format(MotorcycleMsgDialog.AlreadyRegistered, motocycleFromDb?.Model);
                        motorcycleInsertResponse.Errors.Add(strMsg);
                        strMsg.LogWrn(motocycleFromDb?.Model);                        
                    }
                });

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

        public async Task<MotorcycleGetResponse> GetByPlate(MotorcycleGetRequest motorcycleGetRequest)
        {
            try
            {
                var motorcycle = motorcycleGetRequest.Data;
                var motorcycleGetResponse = new MotorcycleGetResponse(motorcycle) { RequestId = motorcycleGetRequest.RequestId };

                await UnitOfWorkExecute(async () =>
                {
                    var motocycleFromDb = await MotorcycleRepository.GetByPlate(motorcycle.Plate);

                    if (motocycleFromDb is null) return;

                    motorcycleGetResponse.Data.Year = motocycleFromDb.Year;
                    motorcycleGetResponse.Data.Model = motocycleFromDb.Model;
                    motorcycleGetResponse.Data.Id = motocycleFromDb.Id;
                });

                return motorcycleGetResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [obter] motocicleta: {Plate}".LogErr(motorcycleGetRequest.Data.Plate);
                exc.Message.LogErr(exc);

                var motorcycleGetResponse = new MotorcycleGetResponse(motorcycleGetRequest.Data);
                motorcycleGetResponse.Errors.Add(exc.Message);
                return motorcycleGetResponse;                
            }         
        }
    }
}
