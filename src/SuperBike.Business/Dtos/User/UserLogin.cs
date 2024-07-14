using SuperBike.Business.UseCases.User;
using System.ComponentModel.DataAnnotations;

namespace SuperBike.Business.Dtos.User
{
    public class UserLogin
    {
        public UserLogin() { }
        public UserLogin(string loginUserName, string accessToken)
        {
            LoginUserName = loginUserName;
            AccessToken = accessToken;
        }
        [Required(ErrorMessage = UserMsgDialog.RequiredLoginEmail)]
        [EmailAddress(ErrorMessage = UserMsgDialog.InvalidEmail)]
        public string LoginUserName { get; set; } = "";

        [MinLength(6, ErrorMessage = UserMsgDialog.MinimalPassword)]
        [Required(ErrorMessage = UserMsgDialog.RequiredPassword)]
        public string Password { get; set; } = "";

        public string? AccessToken { get; private set; }
    }
}
