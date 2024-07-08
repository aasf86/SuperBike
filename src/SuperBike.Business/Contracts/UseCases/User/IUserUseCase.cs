using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;

namespace SuperBike.Business.Contracts.UseCases.User
{
    public interface IUserUseCase
    {
        Task<UserInsertResponse> Insert(UserInsertRequest request);
        Task<UserLoginResponse> Login(UserLoginRequest request);
    }
}
