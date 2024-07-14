using Dapper;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Infrastructure.Repositories.Motorcycle
{
    public class MotorcycleRepository : RepositoryBase<Entity.Motorcycle>, IMotorcycleRepository
    {
        public async Task<Entity.Motorcycle?> GetByPlate(string plate, int? notId = null)
        {
            var sql = $@"
                select * 
                from {nameof(Entity.Motorcycle)} 
                where plate = @plate
            ";
            dynamic param = new { plate };

            if (notId.GetValueOrDefault() > 0)
            {
                sql += " and id <> @notId";
                param = new { plate, notId };
            }

            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<Entity.Motorcycle>(sql, param as object);
        }
    }
}
