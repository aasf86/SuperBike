using Dapper;
using Npgsql;
using SuperBike.Infrastructure.Repositories;
using System.Data;

namespace SuperBike.Consumer.DataAccess
{
    public static class FactoryDataAccess
    {
        public static IDbConnection CreateConnection(string strConnection) => new NpgsqlConnection(strConnection);
    }

    public class DataAccessEvent : IDisposable
    {
        private readonly IDbConnection _connection;
        public DataAccessEvent(IDbConnection connection) => _connection = connection;

        public async Task Insert<T>(T @event)
        {
            var sql = Helpers.StrSql.CreateSqlInsert<T>();
            await _connection.ExecuteScalarAsync(sql, @event);
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            var sql = Helpers.StrSql.CreateSqlSelect<T>("0=0");
            return await _connection.QueryAsync<T>(sql);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
