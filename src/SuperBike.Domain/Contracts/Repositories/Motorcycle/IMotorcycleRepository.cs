using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Domain.Contracts.Repositories.Motorcycle
{
    public interface IMotorcycleRepository : IRepository<Entity.Motorcycle> 
    {
        Task<Entity.Motorcycle> GetByPlate(string plate, int? notId = null);
    }
}
