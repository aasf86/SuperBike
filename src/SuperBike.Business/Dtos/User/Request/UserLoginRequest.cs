namespace SuperBike.Business.Dtos.User.Request
{
    public class UserLoginRequest : RequestBase<UserLogin>
    {
        public UserLoginRequest(UserLogin userLogin) : base(userLogin) { }
    }
}
