using Dapper;
using SuperBike.Domain.Contracts.Repositories;
using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        #region Properties/Fields

        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;

        private string? _sqlInsert;
        private string SqlInsert => _sqlInsert = _sqlInsert ?? Helpers.StrSql.CreateSqlInsert<TEntity>();

        private string? _sqlUdapte;
        private string SqlUdapte => _sqlUdapte = _sqlUdapte ?? Helpers.StrSql.CreateSqlUpdate<TEntity>();

        private string? _sqlDelete;
        private string SqlDelete => _sqlDelete = _sqlDelete?? Helpers.StrSql.CreateSqlDelete<TEntity>();

        private string? _sqlSelect;
        private string SqlSelect => _sqlSelect = _sqlSelect ?? Helpers.StrSql.CreateSqlSelect<TEntity>();

        #endregion

        public virtual void SetTransaction(IDbTransaction dbTransaction) 
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(int id)
        {
            return (await DbTransaction.Connection.ExecuteAsync(SqlDelete, new { id })) > 0;
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return default;            
        }

        public virtual async Task<TEntity?> GetById(int id)
        {            
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<TEntity?>(SqlSelect, new { id });
        }

        public virtual async Task Insert(TEntity entity)
        {            
            entity.Id = await DbTransaction.Connection.ExecuteScalarAsync<int>(SqlInsert, entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await DbTransaction.Connection.ExecuteAsync(SqlUdapte, entity);
        }
    }
}
