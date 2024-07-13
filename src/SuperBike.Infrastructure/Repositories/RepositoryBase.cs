using Dapper;
using SuperBike.Domain.Contracts.Repositories;
using SuperBike.Domain.Entities;
using System.Data;

namespace SuperBike.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        #region Properties/Fields

        private readonly IDbConnection _dbConnection;
        private IDbConnection DbConnection => _dbConnection;

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
        protected RepositoryBase(IDbConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }
        public virtual async Task Delete(int id)
        {
            await DbConnection.ExecuteAsync(SqlDelete, new { id });
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return default;
            //return (await DbConnection.GetAllAsync<TEntity?>()).ToList();
        }

        public virtual async Task<TEntity?> GetById(int id)
        {            
            return await DbConnection.QuerySingleAsync<TEntity?>($"{SqlSelect}", new { id });
        }

        public virtual async Task Insert(TEntity entity)
        {            
            entity.Id = await DbConnection.ExecuteScalarAsync<int>(SqlInsert, entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await DbConnection.ExecuteAsync(SqlUdapte, entity);
        }
    }
}
