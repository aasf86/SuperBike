
namespace SuperBike.Business.Dtos.User.Response
{
    public class UserLoginResponse : ResponseBase<UserLogin>
    {
        public UserLoginResponse(UserLogin userLogin) : base(userLogin) { }
        public UserLoginResponse(UserLogin userLogin, List<string?> erros, Guid? guid) : base(userLogin, erros, guid) { }
        public UserLoginResponse(UserLogin userLogin, string accessToken) : base(userLogin) => AccessToken = accessToken;
        public string AccessToken { get; set; } = "";
    }
}