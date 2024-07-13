using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Infrastructure.Repositories.Motorcycle
{
    public class MotorcycleRepository : RepositoryBase<Entity.Motorcycle>, IMotorcycleRepository
    {
        public MotorcycleRepository(IDbConnection connection) : base(connection) { }

        public Task<Entity.Motorcycle> GetByPlate(string plate)
        {
            throw new NotImplementedException();
        }
    }
}
