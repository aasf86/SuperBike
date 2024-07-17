using Dapper;
using SuperBike.Domain.Contracts.Repositories;
using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Infrastructure.Repositories
{
    public abstract class WriteDataBase<TEntity> : IWriteData<TEntity> where TEntity : EntityBase
    {
        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;

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

        public virtual void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(int id)
        {
            return (await DbTransaction.Connection.ExecuteAsync(SqlDelete, new { id })) > 0;
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
