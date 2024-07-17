using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Domain.Contracts.Repositories
{
    public interface IWriteData<TEntity> where TEntity : EntityBase
    {
        void SetTransaction(IDbTransaction dbTransaction);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task<bool> Delete(int id);
    }
}
