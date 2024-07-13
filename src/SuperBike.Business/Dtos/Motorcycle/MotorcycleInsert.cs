using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Motorcycle;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleInsert
    {
        [Range(MotorcycleRole.YearMinimalValue, int.MaxValue, ErrorMessage = MotorcycleMsgDialog.InvalidYear)]
        public int Year { get; set; }

        [MinLength(MotorcycleRole.ModelMinimalLenth, ErrorMessage = MotorcycleMsgDialog.RequiredModel)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredModel)]
        [MaxLength(100, ErrorMessage = MotorcycleMsgDialog.InvalidModel )]
        public string Model { get; set; } = "";

        [MinLength(MotorcycleRole.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [MaxLength(MotorcycleRole.PlateMaxLenth, ErrorMessage = MotorcycleMsgDialog.InvalidPlate)]
        public string Plate { get; set; } = "";
    }
}
