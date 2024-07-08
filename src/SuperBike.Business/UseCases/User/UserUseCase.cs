using Microsoft.AspNetCore.Identity;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.Dtos.User;
using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;
using System.ComponentModel.DataAnnotations;
using SuperBike.Auth.Config;
using Microsoft.Extensions.Options;

namespace SuperBike.Business.UseCases.User
{
    public class UserUseCase : UseCaseBase, IUserUseCase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public SignInManager<IdentityUser> SignInManager => _signInManager;
        public UserManager<IdentityUser> UserManager => _userManager;
        public JwtOptions JwtOptions => _jwtOptions;

        public UserUseCase(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<UserInsertResponse> Insert(UserInsertRequest userInsertRequest)
        {
            try
            {
                //aasf86 escrever log

                var result = Validate(userInsertRequest.Data);

                if (!result.IsSuccess)
                    return new UserInsertResponse(userInsertRequest.Data, result.Validation.Select(x => x.ErrorMessage).ToList());

                var user = userInsertRequest.Data;

                var identityUser = new IdentityUser
                {
                    UserName = user.LoginUserName,
                    Email = user.LoginUserName,
                    EmailConfirmed = true
                };

                var resultCreate = await UserManager.CreateAsync(identityUser, user.Password);

                if (resultCreate.Succeeded)
                    await _userManager.SetLockoutEnabledAsync(identityUser, false);

                var userInsertResponse = new UserInsertResponse(user);

                if (!resultCreate.Succeeded && resultCreate.Errors.Count() > 0)
                    userInsertResponse.Errors.AddRange(resultCreate.Errors.Select(r => r.Description));

                return userInsertResponse;
            }
            catch (Exception exc)
            {
                //aasf86 escrever log

                var userInsertResponse = new UserInsertResponse(userInsertRequest.Data);
                userInsertResponse.Errors.Add(exc.Message);
#if DEBUG
                userInsertResponse.Exception = exc;
#endif
                return userInsertResponse;
            }
        }
    }
}
