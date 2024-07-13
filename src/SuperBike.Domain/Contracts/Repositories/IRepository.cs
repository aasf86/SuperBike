using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Domain.Contracts.Repositories
{
    public interface IRepository<TEntity> 
        : IWriteData<TEntity>, IReadData<TEntity> 
        where TEntity : EntityBase 
    {
        void SetTransaction(IDbTransaction dbTransaction);
    }
}
