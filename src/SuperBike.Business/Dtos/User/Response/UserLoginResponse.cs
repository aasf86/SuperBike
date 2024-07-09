
namespace SuperBike.Business.Dtos.User.Response
{
    public class UserLoginResponse : ResponseBase<UserLogin>
    {
        public UserLoginResponse(UserLogin userLogin) : base(userLogin) { }
        public UserLoginResponse(UserLogin userLogin, List<string?> erros) : base(userLogin, erros) { }
        public UserLoginResponse(UserLogin userLogin, string accessToken) : base(userLogin) => AccessToken = accessToken;
        public string AccessToken { get; set; } = "";
    }
}