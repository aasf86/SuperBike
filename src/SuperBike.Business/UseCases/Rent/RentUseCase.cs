﻿using Microsoft.Extensions.Logging;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Rent;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Rent;
using SuperBike.Business.UseCases.Renter;
using SuperBike.Domain.Contracts.Repositories.Rent;
using System.Data;
using static SuperBike.Domain.Entities.Rules.Rent;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.UseCases.Rent
{
    public class RentUseCase : UseCaseBase, IRentUseCase
    {
        private readonly IRentRepository _rentRepository;
        private IRentRepository RentRepository => _rentRepository;

        public RentUseCase(
            ILogger<RenterUseCase> logger,
            IRentRepository rentRepository,
            IDbConnection dbConnection) : base(logger, dbConnection)
        {
            _rentRepository = rentRepository;
            TransactionAssigner.Add(_rentRepository.SetTransaction);
        }
        public async Task<ResponseBase<List<RentGet>>> Get(RequestBase<RentGet> rentGetRequest)
        {
            try
            {
#if !DEBUG
                if (!IsInRole(RoleTypeSuperBike.RenterDeliveryman)) throw new UnauthorizedAccessException();
#endif
                "Iniciando [Get] de aluguel: {UserId}".LogInf(rentGetRequest.Data.UserId);

                var rentGet = rentGetRequest.Data;
                var rentGetResponse = ResponseBase.New(new List<RentGet>(), rentGetRequest.RequestId);
                var result = Validate(rentGet);

                await UnitOfWorkExecute(async () =>
                {
                    var listRentFromDb = await RentRepository.GetAll(new { rentGet.UserId });
                    rentGetResponse.Data = listRentFromDb.Select(x => new RentGet
                    {
                        Id = x.Id,
                        RentalplanId = x.RentalplanId,
                        MotorcycleId = x.MotorcycleId,
                        RenterId = x.RenterId,
                        RentalDays = x.RentalDays,
                        InitialDate = x.InitialDate,
                        EndDate = x.EndDate,
                        EndPredictionDate = x.EndPredictionDate,
                        TotalDaysOfRent = x.TotalDaysOfRent,
                        TotalRentalValue = x.TotalRentalValue(),
                        UserId = rentGet.UserId
                    }).ToList();
                });

                return rentGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Get] aluguel: {UserId}".LogErr(rentGetRequest.Data.UserId);
                exc.Message.LogErr(exc);

                var rentGetResponse = ResponseBase.New(new List<RentGet>(), rentGetRequest.RequestId);
                rentGetResponse.Errors.Add(exc.Message);
                return rentGetResponse;
            }
        }

        public async Task<ResponseBase<RentInsert>> Insert(RequestBase<RentInsert> rentInsertRequest)
        {

            try
            {
#if !DEBUG
                if (!IsInRole(RoleTypeSuperBike.RenterDeliveryman)) throw new UnauthorizedAccessException();
#endif

                "Iniciando [Insert] de Aluguel: {UserId}".LogInf(rentInsertRequest.Data.UserId);

                var rentInsert = rentInsertRequest.Data;
                var rentInsertResponse = ResponseBase.New(rentInsert, rentInsertRequest.RequestId);
                var result = Validate(rentInsert);

                if (!result.IsSuccess)
                {
                    rentInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", rentInsertResponse.Errors.ToArray());
                    $"Erro ao alugar motocicleta. '{{UserId}}': {errors} ".LogWrn(rentInsert.UserId);
                    return rentInsertResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var addErros = new Action<string>((string msg) =>
                    {
                        rentInsertResponse.Errors.Add(msg);
                        msg.LogWrn();
                    });

                    var rentalPlanFromDb = (await RentRepository.GetAllPlans()).SingleOrDefault(x => x.Id == rentInsert.RentalplanId);

                    if (rentalPlanFromDb is null)
                    {
                        addErros(RentMsgDialog.NotRegistered);
                        return;
                    }

                    var motocycleAvailable = await RentRepository.MotorcycleAvailable(rentInsert.MotorcycleId);

                    if (!motocycleAvailable)
                    {
                        addErros(RentMsgDialog.AlreadyRented);
                        return;
                    }

                    var rentEntity = new Entity.Rent(
                        rentInsert.MotorcycleId,
                        rentInsert.RenterId,
                        rentalPlanFromDb,
                        rentInsert.RentalDays);


                    await RentRepository.Insert(rentEntity);

                });

                return rentInsertResponse;

            }
            catch (Exception exc)
            {
                "Erro no [Insert] aluguel: {UserId}".LogErr(rentInsertRequest.Data.UserId);
                exc.Message.LogErr(exc);

                var rentInsertResponse = ResponseBase.New(rentInsertRequest.Data, rentInsertRequest.RequestId);
                rentInsertResponse.Errors.Add(exc.Message);
                return rentInsertResponse;
            }
        }
    }
}