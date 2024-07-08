using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Business.Contracts.UseCases.User
{
    public interface IUserUseCase
    {
        Task<UserInsertResponse> Insert(UserInsertRequest request);
    }
}
