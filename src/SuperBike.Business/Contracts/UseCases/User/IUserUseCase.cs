using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.User;

namespace SuperBike.Business.Contracts.UseCases.User
{
    public interface IUserUseCase : IValidators
    {
        Task<ResponseBase<UserInsert>> Insert(RequestBase<UserInsert> request);
        Task<ResponseBase<UserLogin>> Login(RequestBase<UserLogin> request);
    }
}
