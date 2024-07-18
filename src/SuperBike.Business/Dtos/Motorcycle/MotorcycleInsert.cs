using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Motorcycle;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleInsert
    {
        [Range(MotorcycleRule.YearMinimalValue, int.MaxValue, ErrorMessage = MotorcycleMsgDialog.InvalidYear)]
        public int Year { get; set; }

        [MinLength(MotorcycleRule.ModelMinimalLenth, ErrorMessage = MotorcycleMsgDialog.RequiredModel)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredModel)]
        [MaxLength(100, ErrorMessage = MotorcycleMsgDialog.InvalidModel )]
        public string Model { get; set; } = "";

        [MinLength(MotorcycleRule.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [MaxLength(MotorcycleRule.PlateMaxLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        public string Plate { get; set; } = "";
    }
}
