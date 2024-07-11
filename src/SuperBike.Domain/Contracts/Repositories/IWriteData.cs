using SuperBike.Domain.Entities;

namespace SuperBike.Domain.Contracts.Repositories
{
    public interface IWriteData<TEntity> where TEntity : EntityBase
    {
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid guid);
    }
}
