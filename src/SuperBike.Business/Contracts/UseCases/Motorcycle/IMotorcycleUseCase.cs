using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.Motorcycle;

namespace SuperBike.Business.Contracts.UseCases.Motorcycle
{
    public interface IMotorcycleUseCase : IValidators
    {
        Task<ResponseBase<MotorcycleInsert>> Insert(RequestBase<MotorcycleInsert> request);        
        Task<ResponseBase<MotorcycleGet>> GetByPlate(RequestBase<MotorcycleGet> request);
        Task<ResponseBase<MotorcycleUpdate>> Update(RequestBase<MotorcycleUpdate> request);
        Task<ResponseBase<MotorcycleDelete>> Delete(RequestBase<MotorcycleDelete> request);        
    }
}
