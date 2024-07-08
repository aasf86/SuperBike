namespace SuperBike.Business.UseCases.User
{
    public static class UserMsgDialog
    {
        public const string RequiredPassword = "Informe sua senha.";
        public const string InvalidEmail = "Informe um Email/Login válido.";
        public const string RequiredLoginEmail = "O Email/Login é obrigatório.";
        public const string MinimalPassword = "Informe no minimo 6 caracteres para a senha.";
        public const string RequiredPasswordConfirmed = "Informe a senha de confirmação.";
        public const string IsLockedOut = "Essa conta está bloqueada.";
        public const string IsNotAllowed = "Essa conta não tem permissão para fazer login.";
        public const string RequiresTwoFactor = "É necessário confirmar o login no seu segundo fator de autenticação.";
        public const string IncorrectPassword = "Usuário e senha estão incorretos.";
    }
}
