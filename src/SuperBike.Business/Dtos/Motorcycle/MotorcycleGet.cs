using SuperBike.Domain.Contracts.UseCases.Motorcycle;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleGet
    {
        public dynamic Filter { get; private set; } = new ExpandoObject();
    }
}
