using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Rent;

namespace SuperBike.Business.Contracts.UseCases.Rent
{
    public interface IRentUseCase : IValidators
    {
        Task<ResponseBase<RentInsert>> Insert(RequestBase<RentInsert> request);
        Task<ResponseBase<List<RentGet>>> Get(RequestBase<RentGet> request);
    }
}
