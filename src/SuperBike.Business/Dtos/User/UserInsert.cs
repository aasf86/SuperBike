using SuperBike.Business.UseCases.User;
using System.ComponentModel.DataAnnotations;

namespace SuperBike.Business.Dtos.User
{
    public class UserInsert
    {
        [Required(ErrorMessage = UserInserMsgDialog.RequiredLoginEmail)]
        [EmailAddress(ErrorMessage = UserInserMsgDialog.InvalidEmail)]
        public string LoginUserName { get; set; } = "";

        [MinLength(6, ErrorMessage = UserInserMsgDialog.MinimalPassword)]
        [Required(ErrorMessage = UserInserMsgDialog.RequiredPassword)]
        public string Password { get; set; } = "";

        [MinLength(6, ErrorMessage = UserInserMsgDialog.MinimalPassword)]
        [Required(ErrorMessage = UserInserMsgDialog.RequiredPassword2)]
        public string Password2 { get; set; } = "";
    }
}
