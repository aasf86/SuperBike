using Dapper;
using SuperBike.Domain.Contracts.Repositories.Renter;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Infrastructure.Repositories.Renter
{
    public class RenterRepository : WriteDataBase<Entity.Renter>, IRenterRepository
    {
        public async Task<Entity.Renter?> GetByCnpjCpf(string cnpjCpf)
        {
            var sql = $@"
                select * 
                from {nameof(Entity.Renter)} 
                where CnpjCpf = @CnpjCpf
            ";
            
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<Entity.Renter?>(sql, new { cnpjCpf });
        }

        public async Task<Entity.Renter> GetByCNH(string cnh)
        {
            var sql = $@"
                select * 
                from {nameof(Entity.Renter)} 
                where cnh = @cnh
            ";

            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<Entity.Renter?>(sql, new { cnh });
        }
    }
}
