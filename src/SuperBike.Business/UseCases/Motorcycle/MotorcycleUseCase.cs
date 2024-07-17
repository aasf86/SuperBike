using Microsoft.Extensions.Logging;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Motorcycle;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using SuperBike.Domain.Contracts.Services;
using SuperBike.Domain.Events;
using SuperBike.Domain.Events.Motorcycle;
using System.Data;
using static SuperBike.Domain.Entities.Motorcycle;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.UseCases.Motorcycle
{
    public class MotorcycleUseCase : UseCaseBase, IMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private IMotorcycleRepository MotorcycleRepository => _motorcycleRepository;

        private readonly IMessageBroker _messageBroker;
        private IMessageBroker MessageBroker => _messageBroker;
        public MotorcycleUseCase(
            ILogger<MotorcycleUseCase> logger, 
            IMotorcycleRepository motorcycleRepository,
            IDbConnection dbConnection,
            IMessageBroker messageBroker) : base(logger, dbConnection) 
        {
            _motorcycleRepository = motorcycleRepository;
            _messageBroker = messageBroker;
            TransactionAssigner.Add(_motorcycleRepository.SetTransaction);
        }

        public async Task<ResponseBase<MotorcycleInsert>> Insert(RequestBase<MotorcycleInsert> motorcycleInsertRequest)
        {
            try
            {
                //aasf86 Authorize
                //if (!IsInRole(RoleTypeSuperBike.Admin)) throw new UnauthorizedAccessException();

                "Inciando [Insert] de motocicleta: {Plate}".LogInf(motorcycleInsertRequest.Data.Plate);

                var motorcycleInsert = motorcycleInsertRequest.Data;
                var motorcycleInsertResponse = ResponseBase.New(motorcycleInsert, motorcycleInsertRequest.RequestId);
                var result = Validate(motorcycleInsert);

                if (!result.IsSuccess)
                {
                    motorcycleInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", motorcycleInsertResponse.Errors.ToArray());
                    $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(motorcycleInsert.Plate);                    
                    return motorcycleInsertResponse;
                }

                var motorcycleEntity = new Entity.Motorcycle(motorcycleInsert.Year, motorcycleInsert.Model, motorcycleInsert.Plate);

                await UnitOfWorkExecute(async () =>
                {                    
                    var motorcycleFromDb = await MotorcycleRepository.GetByPlate(motorcycleInsert.Plate);

                    if (motorcycleFromDb is not null)
                    {
                        var strMsg = string.Format(MotorcycleMsgDialog.AlreadyRegistered, motorcycleFromDb?.Model);
                        motorcycleInsertResponse.Errors.Add(strMsg);
                        strMsg.LogWrn(motorcycleFromDb?.Model);                        
                        return;
                    }

                    await MotorcycleRepository.Insert(motorcycleEntity);

                    var eventMotorcycleInserted = new MotorcycleInserted
                    {
                        Id = motorcycleEntity.Id,
                        RequestId = motorcycleInsertRequest.RequestId,
                        WhenEvent = motorcycleEntity.Inserted,
                        FromApp = motorcycleInsertRequest.From,
                        Version = motorcycleInsertRequest.Version,
                        Model = motorcycleEntity.Model,
                        Plate = motorcycleEntity.Plate,
                        Year = motorcycleEntity.Year
                    };

                    "Publicando evento: {Plate} {RequestId}".LogInf(motorcycleInsertRequest.Data.Plate, motorcycleInsertRequest.RequestId);

                    await MessageBroker.Publish(eventMotorcycleInserted, Queues.Motorcycle.MOTORCYCLE_INSERTED);
                });

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
                //aasf86 Authorize
                //if (!IsInRole(RoleTypeSuperBike.Admin)) throw new UnauthorizedAccessException();

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
                //aasf86 Authorize
                //if (!IsInRole(RoleTypeSuperBike.Admin)) throw new UnauthorizedAccessException();

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
                //aasf86 Authorize
                //if (!IsInRole(RoleTypeSuperBike.Admin)) throw new UnauthorizedAccessException();

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
