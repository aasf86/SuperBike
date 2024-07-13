namespace SuperBike.Domain.Entities
{
    public class Motorcycle : EntityBase
    {
        public Motorcycle() { }
        public Motorcycle(int year, string model, string plate)
        {
            if (year < MotorcycleRole.YearMinimalValue) throw new InvalidDataException(MotorcycleMsgDialog.InvalidYear);
            if (string.IsNullOrEmpty(model) || model.Length < MotorcycleRole.ModelMinimalLenth) throw new InvalidDataException(MotorcycleMsgDialog.RequiredModel);
            if (string.IsNullOrEmpty(plate) || plate.Length < MotorcycleRole.PlateMinimalLenth) throw new InvalidDataException(MotorcycleMsgDialog.RequiredPlate);
            if (model.Length > MotorcycleRole.ModelMaxLenth) throw new InvalidDataException(MotorcycleMsgDialog.InvalidModel);
            Year = year;
            Model = model;
            Plate = plate;
        }
        public int Year { get; set; }
        public string Model { get; set; } = "";
        public string Plate { get; set; } = "";

        public static class MotorcycleRole
        {
            public const int ModelMinimalLenth = 3;
            public const int ModelMaxLenth = 100;
            public const int PlateMinimalLenth = 7;
            public const int PlateMaxLenth = 7;
            public const int YearMinimalValue = 1;
        }

        public static class MotorcycleMsgDialog
        {
            public const string InvalidYear = "Informe um ano válido.";
            public const string RequiredModel = "Informe o modelo da motocicleta.";
            public const string RequiredPlate = "Informe a placa da motocicleta.";
            public const string InvalidModel = "Informe o modelo até 100 caracteres.";
            public const string InvalidPlate = "Informe a placa com 7 caracteres.";
        }
    }
}
