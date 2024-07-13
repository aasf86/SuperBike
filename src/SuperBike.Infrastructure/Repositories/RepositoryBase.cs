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
        private IDbTransaction DbTransaction => _dbTransaction;

        private Type _typeEntity = typeof(TEntity);
        private Type TypeEntity => _typeEntity;

        private List<string> PropertiesEntity => TypeEntity
            .GetProperties()
            .OrderBy(p => p.Name.ToLower())
            .Where(x => x.Name.ToLower() != "id")
            .Select(x => x.Name)
            .ToList();

        private string? _sqlInsert;
        private string SqlInsert => _sqlInsert = _sqlInsert ?? $@"
            insert into {TypeEntity.Name} ({string.Join(", ", PropertiesEntity)})
            values ({string.Join(", ", PropertiesEntity.Select(x => $"@{x}"))}) returning id
        ";

        private string? _sqlUdapte;
        private string SqlUdapte => _sqlUdapte = _sqlUdapte ?? $@"
            update {TypeEntity.Name}
            set {string.Join(", ", PropertiesEntity.Select(x => $"{x} = @{x}"))}
            where id = @id
        ";

        private string SqlDelete => $@"
            delete from {TypeEntity.Name} 
            where id = @id
        ";

        private string SqlSelect => $@"
            select * 
            from {TypeEntity.Name} 
            where id = @id
        ";

        #endregion

        public virtual void SetTransaction(IDbTransaction dbTransaction) 
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task Delete(int id)
        {
            await DbTransaction.Connection.ExecuteAsync(SqlDelete, new { id });
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return default;
            //return (await DbConnection.GetAllAsync<TEntity?>()).ToList();
        }

        public virtual async Task<TEntity?> GetById(int id)
        {            
            return await DbTransaction.Connection.QuerySingleAsync<TEntity?>($"{SqlSelect}", new { id });
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
