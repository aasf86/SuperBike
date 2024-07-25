using Microsoft.Extensions.Logging;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Renter;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Renter;
using SuperBike.Domain.Contracts.Repositories.Renter;
using System.Data;
using static SuperBike.Domain.Entities.Rules.Renter;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.UseCases.Renter
{
    public class RenterUseCase : UseCaseBase, IRenterUseCase
    {
        private readonly IRenterRepository _renterRepository;
        private IRenterRepository RenterRepository => _renterRepository;

        public RenterUseCase(
            ILogger<RenterUseCase> logger,
            IRenterRepository renterRepository,
            IDbConnection dbConnection) : base(logger, dbConnection)
        {
            _renterRepository = renterRepository;
            TransactionAssigner.Add(_renterRepository.SetTransaction);
        }

        public async Task<ResponseBase<RenterInsert>> Insert(RequestBase<RenterInsert> renterInsertRequest)
        {
            try
            {
#if !DEBUG
                if (!IsInRole(RoleTypeSuperBike.RenterDeliveryman)) throw new UnauthorizedAccessException();
#endif

                "Inciando [Insert] de alugador/entregador: {CnpjCpf}".LogInf(renterInsertRequest.Data.CnpjCpf);

                var renterInsert = renterInsertRequest.Data;
                var renterInsertResponse = ResponseBase.New(renterInsert, renterInsertRequest.RequestId);
                var result = Validate(renterInsert);

                if (!result.IsSuccess)
                {
                    renterInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", renterInsertResponse.Errors.ToArray());
                    $"Alugador/entregador inválido '{{CnpjCpf}}': {errors} ".LogWrn(renterInsert.CnpjCpf);
                    return renterInsertResponse;
                }

                var renterEntity = new Entity.Renter(
                    renterInsert.Name, 
                    renterInsert.CnpjCpf, 
                    renterInsert.DateOfBirth, 
                    renterInsert.CNH, 
                    renterInsert.CNHType, 
                    renterInsert.UserId, 
                    renterInsert.CNHImg);
                
                await UnitOfWorkExecute(async () => 
                {                    
                    var addErros = new Action<string, string>((doc, value) => 
                    {
                        var strErro = $"{RenterMsgDialog.AlreadyRegistered} {doc}: {value}";
                        renterInsertResponse.Errors.Add(strErro);
                        strErro.LogWrn();
                    });

                    var renterFromDbByCnpjCpf = await RenterRepository.GetByCnpjCpf(renterInsert.CnpjCpf);

                    if (renterFromDbByCnpjCpf is not null) 
                    {
                        addErros(nameof(renterInsert.CnpjCpf), renterInsert.CnpjCpf);
                        return;
                    }

                    var renterFromDbByCNH = await RenterRepository.GetByCNH(renterInsert.CNH);

                    if (renterFromDbByCNH is not null)
                    {
                        addErros(nameof(renterInsert.CNH), renterInsert.CNH);
                        return;
                    }

                    var renterFromDbByUserId = await RenterRepository.GetByUserId(renterInsert.UserId);

                    if (renterFromDbByUserId is not null) 
                    {
                        addErros(nameof(renterInsert.UserId), renterInsert.UserId);
                        return;
                    }

                    await RenterRepository.Insert(renterEntity);

                });

                return renterInsertResponse;

            }
            catch (Exception exc)
            {
                "Erro no [Insert] alugador/entregador: {CnpjCpf}".LogErr(renterInsertRequest.Data.CnpjCpf);
                exc.Message.LogErr(exc);

                var renterInsertResponse = ResponseBase.New(renterInsertRequest.Data, renterInsertRequest.RequestId);
                renterInsertResponse.Errors.Add(exc.Message);
                return renterInsertResponse;             
            }            
        }

        public async Task<ResponseBase<RenterUpdate>> Update(RequestBase<RenterUpdate> renterUpdateRequest)
        {
            try
            {
#if !DEBUG
                if (!IsInRole(RoleTypeSuperBike.RenterDeliveryman)) throw new UnauthorizedAccessException();
#endif

                "Inciando [Update] de imagem da CNH alugador/entregador UserId: {UserId}".LogInf(renterUpdateRequest.Data.UserId);

                var renterUpdate = renterUpdateRequest.Data;
                var renterUpdateResponse = ResponseBase.New(renterUpdate, renterUpdateRequest.RequestId);
                var result = Validate(renterUpdate);

                if (!result.IsSuccess)
                {
                    renterUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", renterUpdateResponse.Errors.ToArray());
                    $"Alugador/entregador inválido '{{UserId}}': {errors} ".LogWrn(renterUpdate.UserId);
                    return renterUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var renterFromDb = await RenterRepository.GetByUserId(renterUpdate.UserId);

                    if (renterFromDb is null) 
                    {
                        renterUpdateResponse.Errors.Add(RenterMsgDialog.NotRegistered);
                        return;
                    }

                    renterFromDb.SetCNHImg(renterUpdate.CNHImg);

                    await RenterRepository.Update(renterFromDb);

                });

                return renterUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Update] alugador/entregador: {UserId}".LogErr(renterUpdateRequest.Data.UserId);
                exc.Message.LogErr(exc);

                var renterInsertResponse = ResponseBase.New(renterUpdateRequest.Data, renterUpdateRequest.RequestId);
                renterInsertResponse.Errors.Add(exc.Message);
                return renterInsertResponse;
            }
        }
    }
}
