using SuperBike.Domain.Contracts.UseCases.Motorcycle;
using System.ComponentModel.DataAnnotations;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleUpdate
    {
        [MinLength(Entity.Motorcycle.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        public string Plate { get; private set; } = "";
    }
}
