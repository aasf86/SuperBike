using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Motorcycle;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleUpdate
    {
        [MinLength(MotorcycleRole.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        public string Plate { get; private set; } = "";
    }
}
