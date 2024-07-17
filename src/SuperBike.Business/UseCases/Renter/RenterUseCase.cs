using Microsoft.Extensions.Logging;
using SuperBike.Auth.Business;
using SuperBike.Business.Contracts.UseCases.Renter;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Renter;
using SuperBike.Business.UseCases.Validators;
using SuperBike.Domain.Contracts.Repositories.Renter;
using System.Data;
using static SuperBike.Domain.Entities.Motorcycle;
using static SuperBike.Domain.Entities.Renter;
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
                //aasf86 Authorize
                //if (!IsInRole(RoleTypeSuperBike.RenterDeliveryman)) throw new UnauthorizedAccessException();

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

                var renterEntity = new Entity.Renter(renterInsert.Name, renterInsert.CnpjCpf, renterInsert.DateOfBirth, renterInsert.CNH, renterInsert.CNHType, renterInsert.CNHImg);
                
                //aasf86 Relacionar com o usuario logado no request
                //renterEntity.UserId = renterInsert.UserId;

                await UnitOfWorkExecute(async () => 
                {                    
                    var addErros = new Action<string, string>((doc, value) => 
                    {
                        var strErro = $"{RenterMsgDialog.AlreadyRegistered} {doc}: {value}";
                        renterInsertResponse.Errors.Add(strErro);
                        strErro.LogWrn();
                    });

                    //aasf86 Verificar se usuario já está relacionado a algum alugador/entregador
                    //var renterFromDbByUserId = await RenterRepository.GetByUserId(renterInsert.UserId);

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

                    await RenterRepository.Insert(renterEntity);

                });

                return renterInsertResponse;

            }
            catch (Exception exc)
            {
                "Erro no [Insert] alugador/entregador: {CnpjCpf}".LogErr(renterInsertRequest.Data.CnpjCpf);
                exc.Message.LogErr(exc);

                var motorcycleInsertResponse = ResponseBase.New(renterInsertRequest.Data, renterInsertRequest.RequestId);
                motorcycleInsertResponse.Errors.Add(exc.Message);
                return motorcycleInsertResponse;             
            }
            
        }

        public Task<ResponseBase<RenterUpdate>> Update(RequestBase<RenterUpdate> request)
        {
            throw new NotImplementedException();
        }
    }
}
