using SuperBike.Domain.Entities;

namespace SuperBike.Domain.Contracts.Repositories
{
    public interface IRepository<TEntity> 
        : IWriteData<TEntity>, IReadData<TEntity> 
        where TEntity : EntityBase { }
}
