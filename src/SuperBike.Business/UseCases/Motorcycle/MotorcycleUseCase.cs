using Microsoft.Extensions.Logging;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Motorcycle;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
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

        public async Task<ResponseBase<MotorcycleInsert>> Insert(RequestBase<MotorcycleInsert> motorcycleInsertRequest)
        {
            try
            {
                "Inciando [Insert] de motocicleta: {Plate}".LogInf(motorcycleInsertRequest.Data.Plate);

                var motocycleInsert = motorcycleInsertRequest.Data;
                var motorcycleInsertResponse = ResponseBase.New(motocycleInsert, motorcycleInsertRequest.RequestId);
                var result = Validate(motocycleInsert);

                if (!result.IsSuccess)
                {
                    motorcycleInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", motorcycleInsertResponse.Errors.ToArray());
                    $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(motocycleInsert.Plate);                    
                    return motorcycleInsertResponse;
                }

                await UnitOfWorkExecute(async () =>
                {                    
                    var motorcycleFromDb = await MotorcycleRepository.GetByPlate(motocycleInsert.Plate);

                    if (motorcycleFromDb is null)
                    {
                        await MotorcycleRepository.Insert(new Entity.Motorcycle(motocycleInsert.Year, motocycleInsert.Model, motocycleInsert.Plate));
                    }
                    else
                    {
                        var strMsg = string.Format(MotorcycleMsgDialog.AlreadyRegistered, motorcycleFromDb?.Model);
                        motorcycleInsertResponse.Errors.Add(strMsg);
                        strMsg.LogWrn(motorcycleFromDb?.Model);                        
                    }
                });

//aasf86 Inserir na fila rabit MQ
#warning >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
#warning >>>>>>>>>>>>>>>>>>>>>>>>>Inserir na fila rabit MQ<<<<<<<<<<<<<<<<<<<<<<<<<
#warning >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                return motorcycleInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Insert] motocicleta: {Plate}".LogErr(motorcycleInsertRequest.Data.Plate);
                exc.Message.LogErr(exc);

                var motorcycleInsertResponse = ResponseBase.New(motorcycleInsertRequest.Data, motorcycleInsertRequest.RequestId);
                motorcycleInsertResponse.Errors.Add(exc.Message);
                return motorcycleInsertResponse;
            }            
        }

        public async Task<ResponseBase<MotorcycleGet>> GetByPlate(RequestBase<MotorcycleGet> motorcycleGetRequest)
        {
            try
            {
                "Inciando [GetByPlate] de motocicleta: {Plate}".LogInf(motorcycleGetRequest.Data.Plate);                

                var motorcycleGet = motorcycleGetRequest.Data;
                var motorcycleGetResponse = ResponseBase.New(motorcycleGet, motorcycleGetRequest.RequestId);
                var result = Validate(motorcycleGet);                

                if (!result.IsSuccess)
                {
                    motorcycleGetResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", motorcycleGetResponse.Errors.ToArray());
                    $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(motorcycleGet.Plate);
                    return motorcycleGetResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var motocycleFromDb = await MotorcycleRepository.GetByPlate(motorcycleGet.Plate);

                    if (motocycleFromDb is null)
                    {
                        motorcycleGetResponse.Errors.Add(MotorcycleMsgDialog.NotFound);
                        $"{MotorcycleMsgDialog.NotFound} '{{Plate}}'".LogWrn();
                        return;
                    }

                    motorcycleGetResponse.Data.Year = motocycleFromDb.Year;
                    motorcycleGetResponse.Data.Model = motocycleFromDb.Model;
                    motorcycleGetResponse.Data.Id = motocycleFromDb.Id;
                });

                return motorcycleGetResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [GetByPlate] motocicleta: {Plate}".LogErr(motorcycleGetRequest.Data.Plate);
                exc.Message.LogErr(exc);

                var motorcycleGetResponse = ResponseBase.New(motorcycleGetRequest.Data, motorcycleGetRequest.RequestId);
                motorcycleGetResponse.Errors.Add(exc.Message);
                return motorcycleGetResponse;                
            }         
        }

        public async Task<ResponseBase<MotorcycleUpdate>> Update(RequestBase<MotorcycleUpdate> motorcycleUpdateRequest)
        {
            try
            {
                "Inciando [Update] de motocicleta: {Plate}".LogInf(motorcycleUpdateRequest.Data.Plate);

                var motorcycleUpdate = motorcycleUpdateRequest.Data;
                var motorcycleUpdateResponse = ResponseBase.New(motorcycleUpdate, motorcycleUpdateRequest.RequestId);
                var result = Validate(motorcycleUpdate);

                if (!result.IsSuccess)
                {
                    motorcycleUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", motorcycleUpdateResponse.Errors.ToArray());
                    $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(motorcycleUpdate.Plate);
                    return motorcycleUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var motorcycleFromDb = await MotorcycleRepository.GetByPlate(motorcycleUpdate.Plate, motorcycleUpdate.Id);

                    if (motorcycleFromDb is not null)
                    {
                        var strMsg = string.Format(MotorcycleMsgDialog.AlreadyRegistered, motorcycleFromDb.Model);
                        motorcycleUpdateResponse.Errors.Add(strMsg);
                        strMsg.LogWrn(motorcycleFromDb?.Model);
                        return;
                    }

                    motorcycleFromDb = await MotorcycleRepository.GetById(motorcycleUpdate.Id);

                    if (motorcycleFromDb is null)
                    {
                        motorcycleUpdateResponse.Errors.Add(MotorcycleMsgDialog.NotFound);
                        $"{MotorcycleMsgDialog.NotFound} 'Id: {{Id}}'".LogWrn(motorcycleUpdate.Id);
                        return;
                    }

                    var newMotorcycleUpdate = new Entity.Motorcycle
                    (
                        motorcycleFromDb.Year,
                        motorcycleFromDb.Model,
                        motorcycleUpdate.Plate
                    )
                    { Id = motorcycleUpdate.Id };

                    await MotorcycleRepository.Update(newMotorcycleUpdate);
                });

                return motorcycleUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] motocicleta: {Plate}".LogErr(motorcycleUpdateRequest.Data.Plate);
                exc.Message.LogErr(exc);

                var motorcycleUpdateResponse = ResponseBase.New(motorcycleUpdateRequest.Data, motorcycleUpdateRequest.RequestId);
                motorcycleUpdateResponse.Errors.Add(exc.Message);
                return motorcycleUpdateResponse;                
            }
        }

        public async Task<ResponseBase<MotorcycleDelete>> Delete(RequestBase<MotorcycleDelete> motorcycleDeleteRequest)
        {
            try
            {
                "Inciando [Delete] de motocicleta: {Id}".LogInf(motorcycleDeleteRequest.Data.Id);

                var motorcycleDelete = motorcycleDeleteRequest.Data;
                var motorcycleDeleteResponse = ResponseBase.New(motorcycleDelete, motorcycleDeleteRequest.RequestId);
                var result = Validate(motorcycleDelete);

                if (!result.IsSuccess)
                {
                    motorcycleDeleteResponse.Errors.Add(MotorcycleMsgDialog.InvalidId);
                    return motorcycleDeleteResponse;
                }

                await UnitOfWorkExecute(async () => 
                {
                    var deleted = await MotorcycleRepository.Delete(motorcycleDelete.Id);
                    if (!deleted)
                        motorcycleDeleteResponse.Errors.Add(MotorcycleMsgDialog.NotFound);                    
                });

                return motorcycleDeleteResponse;
            }
            catch (Exception exc)
            {
                "Erro [Delete] motocicleta: {Id}".LogErr(motorcycleDeleteRequest.Data.Id);
                exc.Message.LogErr(exc);

                var motorcycleDeleteResponse = ResponseBase.New(motorcycleDeleteRequest.Data, motorcycleDeleteRequest.RequestId);
                motorcycleDeleteResponse.Errors.Add(exc.Message);
                return motorcycleDeleteResponse;
            }             
        }
    }
}
