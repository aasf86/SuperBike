namespace SuperBike.Domain.Entities
{
    public partial class Renter
    {
        public static class RenterRole
        {
            public const int NameMinimalLenth = 3;
            public static string[] TypesCNHAllowed = ["A", "B", "AB"];
            public const int CNHMinimalLenth = 11;
            public const int CNHMaxLenth = 11;
            public const int CnpjCpfMinimalLenth = 11;
            public const int CnpjCpfMaxLenth = 14;
            public static string[] ImagesAllowedContentType = ["image/bmp", "image/png"];            
        }

        public static class RenterMsgDialog
        {
            public const string RequiredName = "Informe o nome.";
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
        }        
    }
}
