using SuperBike.Domain.Entities;

namespace SuperBike.Domain.Contracts.Repositories
{
    public interface IReadData<TEntity> where TEntity : EntityBase
    {
        Task<TEntity?> GetByGuid(Guid guid);
        Task<List<TEntity?>> GetAll(dynamic filter);
    }
}
