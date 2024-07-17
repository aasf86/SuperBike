using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Domain.Contracts.Repositories
{
    public interface IReadData<TEntity> where TEntity : EntityBase
    {
        void SetTransaction(IDbTransaction dbTransaction);
        Task<TEntity?> GetById(int id);
        Task<List<TEntity?>> GetAll(dynamic filter);
    }
}
