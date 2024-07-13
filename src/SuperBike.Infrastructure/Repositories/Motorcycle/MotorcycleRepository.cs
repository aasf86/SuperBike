using Dapper;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Infrastructure.Repositories.Motorcycle
{
    public class MotorcycleRepository : RepositoryBase<Entity.Motorcycle>, IMotorcycleRepository
    {
        public async Task<Entity.Motorcycle?> GetByPlate(string plate)
        {
            var sql = $@"
                select * 
                from {nameof(Entity.Motorcycle)} 
                where plate = @plate
            ";

            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<Entity.Motorcycle>(sql, new { plate });            
        }
    }
}
