namespace SuperBike.Domain.Entities.Rules
{
    public partial class Rent
    {
        public static class RentRule
        {
            /*
            public const int NameMinimalLenth = 3;
            public static string[] TypesCNHAllowed = ["A", "B", "AB"];
            public const int CNHMinimalLenth = 11;
            public const int CNHMaxLenth = 11;
            public const int CnpjCpfMinimalLenth = 11;
            public const int CnpjCpfMaxLenth = 14;
            public static string[] ImagesAllowedContentType = ["image/bmp", "image/png"];
            */
        }

        public static class RentMsgDialog
        {
            public const string RequiredMotorcycle = "Informe a motocicleta a ser alugada.";
            public const string RequiredRenter = "Informe o alugador/entregador.";
            public const string RequiredRentalPlan = "Informe o plano de aluguel.";
            public const string InvalidRentalDays = "Informe uma quantidade de dia(s) válido.";

            /*
            public const string InvalidName = "Informe o nome com mais de 2 caracteres.";
            public const string RequiredCnpjCpf = "Informe o CnpjCpf.";
            public const string InvalidCnpjCpf = "CnpjCpf inválido.";
            public const string InvalidDateOfBirth = "Informe uma data de nascimento válida.";
            public const string RequiredCNH = "Informe a CNH.";
            public const string InvalidCNH = "CNH inválida.";
            public const string RequiredCNHType = "Informe o tipo de CNH.";
            public const string InvalidCNHType = "Tipo de CNH não permitido.";
            public const string AlreadyRegistered = "Alugador/entregador já cadastrado.";
            public const string RequiredUserId = "Obrigatório usuário ao alugador/entregador.";
            public const string InvalidContentType = "Tipo de arquivo inválido.";
            public const string RequiredCNHImg = "Informe a imagem da CNH.";
            public const string MotRegistered = "Alugador/entregador não registrado.";
            */
        }
    }
}
