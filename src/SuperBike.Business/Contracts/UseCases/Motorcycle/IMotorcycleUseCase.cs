using SuperBike.Business.Dtos.Motorcycle.Request;
using SuperBike.Business.Dtos.Motorcycle.Response;

namespace SuperBike.Business.Contracts.UseCases.Motorcycle
{
    public interface IMotorcycleUseCase : IValidators
    {
        Task<MotorcycleInsertResponse> Insert(MotorcycleInsertRequest request);
        Task<MotorcycleGetResponse> GetByPlate(MotorcycleGetRequest request);
        /*Task Update(RequestUpdate request);
          Task Delete(RequestDelete request);
          Task GetByGuid(RequestGetGuid request);
          Task GetAll(RequestGetAll request);*/
    }
}
