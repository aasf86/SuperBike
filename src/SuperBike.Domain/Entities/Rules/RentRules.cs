namespace SuperBike.Domain.Entities.Rules
{
    public partial class Rent
    {
        public static class RentRule
        {
            public const int DaysMinimal = 1;
            public const string CNHTypeAllowed = "A";
        }

        public static class RentMsgDialog
        {
            public const string RequiredMotorcycle = "Informe a motocicleta a ser alugada.";
            public const string RequiredRenter = "Informe o alugador/entregador.";
            public const string RequiredRentalPlan = "Informe o plano de aluguel.";
            public const string InvalidRentalDays = "Informe uma quantidade de dia(s) válido.";
            public const string NotRegistered = "Plano de aluguel não registrado.";
            public const string AlreadyRented = "Motocicleta já está alugada, informe outra.";
            public const string CNHTypeNotAllowed = "Tipo/categoria de CNH não permitido para aluguel.";
        }
    }
}
