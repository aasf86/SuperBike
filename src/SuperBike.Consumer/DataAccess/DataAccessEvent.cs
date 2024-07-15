using Dapper;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
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

        public async Task Insert(object @event)
        {
            var type = @event.GetType();
            var propertiesEntity = type
                .GetProperties()
                .OrderBy(p => p.Name.ToLower())
                .Where(x => x.Name.ToLower() != "id")
                .Select(x => x.Name)
                .ToList();
            var attrTable = type.GetCustomAttributes(true).SingleOrDefault(x => x is TableAttribute) as TableAttribute;


            var sql = $@"
                insert into {attrTable.Name} ({string.Join(", ", propertiesEntity)})
                values ({string.Join(", ", propertiesEntity.Select(x => $"@{x}"))}) returning id";

            await _connection.ExecuteScalarAsync(sql, @event);
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            var type = typeof(T);
            var attrTable = type.GetCustomAttributes(true).SingleOrDefault(x => x is TableAttribute) as TableAttribute;

            var sql = $@"
                select * 
                from {attrTable.Name}
            ";

            return await _connection.QueryAsync<T>(sql);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
