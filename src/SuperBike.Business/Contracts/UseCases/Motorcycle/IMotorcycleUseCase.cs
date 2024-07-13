using SuperBike.Business.Dtos.Motorcycle.Request;
using SuperBike.Business.Dtos.Motorcycle.Response;

namespace SuperBike.Business.Contracts.UseCases.Motorcycle
{
    public interface IMotorcycleUseCase
    {
        Task<MotorcycleInsertResponse> Insert(MotorcycleInsertRequest request);
      /*Task Update(RequestUpdate request);
        Task Delete(RequestDelete request);
        Task GetByGuid(RequestGetGuid request);
        Task GetAll(RequestGetAll request);*/
    }
}
