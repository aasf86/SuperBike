using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Domain.Contracts.Repositories.Motorcycle
{
    public interface IMotorcycleRepository : IRepository<Entity.Motorcycle> 
    {
        Task<Entity.Motorcycle> GetByPlate(string plate);
    }
}
