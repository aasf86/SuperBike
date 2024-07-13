using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Motorcycle;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleUpdate
    {
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [MaxLength(MotorcycleRole.PlateMaxLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        [MinLength(MotorcycleRole.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        public string Plate { get; set; } = "";
    }
}
