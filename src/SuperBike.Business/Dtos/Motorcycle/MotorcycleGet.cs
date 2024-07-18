using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Motorcycle;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleGet
    {        
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [MaxLength(MotorcycleRule.PlateMaxLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        [MinLength(MotorcycleRule.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        public string Plate { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public int Id { get; set; }
    }
}
