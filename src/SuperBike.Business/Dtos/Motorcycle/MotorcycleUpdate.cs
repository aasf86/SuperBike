using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Motorcycle;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleUpdate
    {
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [MaxLength(MotorcycleRule.PlateMaxLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        [MinLength(MotorcycleRule.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        public string Plate { get; set; } = "";

        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredId)]        
        [Range(1, int.MaxValue, ErrorMessage = MotorcycleMsgDialog.RequiredId)]
        public int Id { get; set; }
    }
}
