using SuperBike.Domain.Contracts.UseCases.Motorcycle;
using System.ComponentModel.DataAnnotations;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.Dtos.Motorcycle
{
    public class MotorcycleInsert
    {
        [Range(Entity.Motorcycle.YearMinimalValue, int.MaxValue, ErrorMessage = MotorcycleMsgDialog.InvalidYear)]
        public int Year { get; private set; }

        [MinLength(Entity.Motorcycle.ModelMinimalLenth, ErrorMessage = MotorcycleMsgDialog.RequiredModel)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredModel)]
        public string Model { get; private set; } = "";

        [MinLength(Entity.Motorcycle.PlateMinimalLenth, ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        [Required(ErrorMessage = MotorcycleMsgDialog.RequiredPlate)]
        public string Plate { get; private set; } = "";
    }
}
