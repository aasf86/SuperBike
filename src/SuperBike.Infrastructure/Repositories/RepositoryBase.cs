using Dapper.Contrib.Extensions;
using SuperBike.Domain.Contracts.Repositories;
using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IDbConnection _connection;
        private IDbConnection DbConnection => _connection;

        protected RepositoryBase(IDbConnection connection) 
        { 
            _connection = connection;
        }
        public virtual async Task Delete(Guid guid)
        {
            await DbConnection.DeleteAsync((TEntity)new EntityBase(guid));
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return (await DbConnection.GetAllAsync<TEntity?>()).ToList();
        }

        public virtual async Task<TEntity?> GetByGuid(Guid guid)
        {
            return await DbConnection.GetAsync<TEntity>(new EntityBase(guid));
        }

        public virtual async Task Insert(TEntity entity)
        {
            await DbConnection.InsertAsync(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await DbConnection.UpdateAsync(entity);
        }
    }
}
