using Dapper;
using SuperBike.Domain.Contracts.Repositories.Renter;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Infrastructure.Repositories.Renter
{
    public class RenterRepository : WriteDataBase<Entity.Renter>, IRenterRepository
    {
        public async Task<Entity.Renter?> GetByCnpjCpf(string cnpjCpf)
        {
            var sql = Helpers.StrSql.CreateSqlSelect<Entity.Renter>("CnpjCpf = @CnpjCpf");
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<Entity.Renter?>(sql, new { cnpjCpf });
        }

        public async Task<Entity.Renter> GetByCNH(string cnh)
        {
            var sql = Helpers.StrSql.CreateSqlSelect<Entity.Renter>("cnh = @cnh");
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<Entity.Renter?>(sql, new { cnh });
        }

        public async Task<Entity.Renter> GetByUserId(string userId)
        {
            var sql = Helpers.StrSql.CreateSqlSelect<Entity.Renter>("userid = @userid");
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<Entity.Renter?>(sql, new { userId });
        }
    }
}
