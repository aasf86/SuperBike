using Dapper;
using SuperBike.Domain.Contracts.Repositories;
using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Infrastructure.Repositories
{
    public abstract class ReadDataBase<TEntity> : IReadData<TEntity> where TEntity : EntityBase
    {
        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;

        private Type _typeEntity = typeof(TEntity);
        private Type TypeEntity => _typeEntity;

        private string? _SqlSelect;
        private string SqlSelect => _SqlSelect = _SqlSelect ?? $@"
            select * 
            from {TypeEntity.Name} 
            where id = @id
        ";

        public virtual void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            //implementar filter, no futuro, pois até o momento não é necessário
            return (await DbTransaction.Connection.QueryAsync<TEntity?>(SqlSelect, new { filter })).ToList();
        }

        public virtual async Task<TEntity?> GetById(int id)
        {
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<TEntity?>($"{SqlSelect}", new { id });
        }
    }
}
