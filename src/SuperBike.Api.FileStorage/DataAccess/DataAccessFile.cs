using Dapper;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace SuperBike.Api.FileStorage.DataAccess
{
    public static class FactoryDataAccess
    {
        public static IDbConnection CreateConnection(string strConnection) => new NpgsqlConnection(strConnection);
    }

    public class DataAccessFile : IDisposable
    {
        private readonly IDbConnection _connection;
        public DataAccessFile(IDbConnection connection) => _connection = connection;

        public async Task Insert(object file)
        {
            var type = file.GetType();
            var propertiesEntity = type
                .GetProperties()
                .OrderBy(p => p.Name.ToLower())
                .Where(x => x.Name.ToLower() != "id")
                .Select(x => x.Name)
                .ToList();
            var attrTable = type.GetCustomAttributes(true).SingleOrDefault(x => x is TableAttribute) as TableAttribute;


            var sql = $@"
                insert into {(attrTable?.Name ?? type.Name)} ({string.Join(", ", propertiesEntity)})
                values ({string.Join(", ", propertiesEntity.Select(x => $"@{x}"))}) returning id";

            await _connection.ExecuteScalarAsync(sql, file);
        }

        public async Task<T> GetByKey<T>(string key)
        {
            var type = typeof(T);
            var attrTable = type.GetCustomAttributes(true).SingleOrDefault(x => x is TableAttribute) as TableAttribute;

            var sql = $@"
                select * 
                from {(attrTable?.Name ?? type.Name)}
                where key = @key
            ";

            return await _connection.QuerySingleOrDefaultAsync<T>(sql, new { key });
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}

