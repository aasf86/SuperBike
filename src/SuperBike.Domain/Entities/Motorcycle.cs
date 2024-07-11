using SuperBike.Domain.Contracts.UseCases.Motorcycle;

namespace SuperBike.Domain.Entities
{
    public class Motorcycle : EntityBase
    {
        public const int ModelMinimalLenth = 3;
        public const int PlateMinimalLenth = 7;
        public const int YearMinimalValue = 1;

        public Motorcycle(int year, string model, string plate)
        {
            if (year < YearMinimalValue) throw new InvalidDataException(MotorcycleMsgDialog.InvalidYear);
            if (string.IsNullOrEmpty(model) || model.Length < ModelMinimalLenth) throw new InvalidDataException(MotorcycleMsgDialog.RequiredModel);
            if (string.IsNullOrEmpty(plate) || plate.Length < PlateMinimalLenth) throw new InvalidDataException(MotorcycleMsgDialog.RequiredPlate);

            Year = year;
            Model = model;
            Plate = plate;
        }
        public int Year { get; private set; }
        public string Model { get; private set; } = "";
        public string Plate { get; private set; } = "";
    }
}
