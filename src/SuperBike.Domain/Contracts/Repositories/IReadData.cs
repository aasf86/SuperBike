using SuperBike.Domain.Entities;

namespace SuperBike.Domain.Contracts.Repositories
{
    public interface IReadData<TEntity> where TEntity : EntityBase
    {
        Task<TEntity?> GetById(int id);
        Task<List<TEntity?>> GetAll(dynamic filter);
    }
}
