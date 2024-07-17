using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Renter;

namespace SuperBike.Business.Contracts.UseCases.Renter
{
    public interface IRenterUseCase : IValidators
    {
        Task<ResponseBase<RenterInsert>> Insert(RequestBase<RenterInsert> request);
        Task<ResponseBase<RenterUpdate>> Update(RequestBase<RenterUpdate> request);
    }
}
