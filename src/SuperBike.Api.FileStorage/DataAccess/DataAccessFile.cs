using Dapper;
using Npgsql;
using SuperBike.Infrastructure.Repositories;
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

        public async Task Insert<T>(T file)
        {
            var sql = Helpers.StrSql.CreateSqlInsert<T>();
            await _connection.ExecuteScalarAsync(sql, file);
        }

        public async Task<T> GetByKey<T>(string key)
        {
            var sql = Helpers.StrSql.CreateSqlSelect<T>("key = @key");
            return await _connection.QuerySingleOrDefaultAsync<T>(sql, new { key });
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}

