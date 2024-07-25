using Entity = SuperBike.Domain.Entities;
using SuperBike.Domain.Entities.ValueObjects.Rent;

namespace SuperBike.Domain.Contracts.Repositories.Rent
{
    public interface IRentRepository : IRepository<Entity.Rent> 
    {
        Task<List<RentalPlan>> GetAllPlans();

        Task<bool> MotorcycleAvailable(int motorcycleId);
    }
}
