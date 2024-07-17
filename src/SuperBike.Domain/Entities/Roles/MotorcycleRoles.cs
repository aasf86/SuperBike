namespace SuperBike.Domain.Entities
{
    public partial class Motorcycle
    {
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
            public const string AlreadyRegistered = "Motocicleta já cadastrada para o modelo {0}.";
            public const string RequiredId = "Id obrigatório.";
            public const string NotFound = "Motocicleta não encontrada.";
            public const string InvalidId = "Id motocicleta inválido.";
        }
    }
}
