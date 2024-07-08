using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperBike.Auth.Config;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;

namespace SuperBike.Business.UseCases.User
{
    public class UserUseCase : UseCaseBase, IUserUseCase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<UserUseCase> _logger;

        private SignInManager<IdentityUser> SignInManager => _signInManager;
        private UserManager<IdentityUser> UserManager => _userManager;
        private JwtOptions JwtOptions => _jwtOptions;
        private ILogger<UserUseCase> Logger => _logger;

        public UserUseCase(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtOptions> jwtOptions,
            ILogger<UserUseCase> logger) : base(logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }

        public async Task<UserInsertResponse> Insert(UserInsertRequest userInsertRequest)
        {
            try
            {
                "Iniciando [insert] de usuário: {LoginUserName}".LogInf(userInsertRequest.Data.LoginUserName);

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
                {
                    userInsertResponse.Errors.AddRange(resultCreate.Errors.Select(r => r.Description));
                    var errors = string.Join("\n", userInsertResponse.Errors.ToArray());
                    $"Validações do Identity Erros: {errors}".LogWrn();
                }

                return userInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [inserir] usuário: {LoginUserName}".LogErr(userInsertRequest.Data.LoginUserName);
                exc.Message.LogErr(exc);

                var userInsertResponse = new UserInsertResponse(userInsertRequest.Data);
                userInsertResponse.Errors.Add(exc.Message);
                userInsertResponse.Exception = exc;
                return userInsertResponse;
            }
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest)
        {
            try
            {
                "Iniciando 'login' de usuário: {LoginUserName}".LogInf(userLoginRequest.Data.LoginUserName);

                var userLoginResponse = new UserLoginResponse(userLoginRequest.Data);


                return userLoginResponse;
            }
            catch (Exception exc)
            {
                "Erro no [login] usuário: {LoginUserName}".LogErr(userLoginRequest.Data.LoginUserName);
                exc.Message.LogErr(exc);

                var userLoginResponse = new UserLoginResponse(userLoginRequest.Data);
                userLoginResponse.Errors.Add(exc.Message);
                userLoginResponse.Exception = exc;
                return userLoginResponse;
            }
        }
    }
}
