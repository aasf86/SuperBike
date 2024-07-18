namespace SuperBike.Domain.Entities
{
    public partial class Motorcycle : EntityBase
    {
        public Motorcycle() { }
        public Motorcycle(int year, string model, string plate)
        {
            if (year < MotorcycleRule.YearMinimalValue) throw new InvalidDataException(MotorcycleMsgDialog.InvalidYear);
            if (string.IsNullOrEmpty(model) || model.Length < MotorcycleRule.ModelMinimalLenth) throw new InvalidDataException(MotorcycleMsgDialog.RequiredModel);
            if (string.IsNullOrEmpty(plate) || plate.Length < MotorcycleRule.PlateMinimalLenth) throw new InvalidDataException(MotorcycleMsgDialog.RequiredPlate);
            if (model.Length > MotorcycleRule.ModelMaxLenth) throw new InvalidDataException(MotorcycleMsgDialog.InvalidModel);
            Year = year;
            Model = model;
            Plate = plate;
        }
        public int Year { get; private set; }
        public string Model { get; private set; } = "";
        public string Plate { get; private set; } = "";
    }
}
