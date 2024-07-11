using SuperBike.Domain.Contracts.UseCases.Motorcycle;
using System.ComponentModel.DataAnnotations;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleDelete
    {
        public Guid Guid { get; private set; } = Guid.NewGuid();
    }
}
