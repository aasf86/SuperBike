using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Domain.Contracts.Repositories.Renter
{
    public interface IRenterRepository : IWriteData<Entity.Renter> 
    {
        Task<Entity.Renter> GetByCnpjCpf(string cnpjCpf);
        Task<Entity.Renter> GetByCNH(string cnh);
        Task<Entity.Renter> GetByUserId(string userId);
    }
}
