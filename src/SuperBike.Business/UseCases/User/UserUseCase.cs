using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperBike.Auth.Config;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.Dtos.User;
using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        public UserUseCase(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtOptions> jwtOptions,            
            ILogger<UserUseCase> logger) : base(logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
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

                var user = userLoginRequest.Data;
                var userLoginResponse = new UserLoginResponse(new UserLogin { LoginUserName = user.LoginUserName });

                var result = await _signInManager.PasswordSignInAsync(user.LoginUserName, user.Password, false, true);
                if (result.Succeeded)
                    return await GenerateToken(user.LoginUserName);

                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                        userLoginResponse.Errors.Add(UserMsgDialog.IsLockedOut);
                    else if (result.IsNotAllowed)
                        userLoginResponse.Errors.Add(UserMsgDialog.IsNotAllowed);
                    else if (result.RequiresTwoFactor)
                        userLoginResponse.Errors.Add(UserMsgDialog.RequiresTwoFactor);
                    else
                        userLoginResponse.Errors.Add(UserMsgDialog.IncorrectPassword);
                }

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

        private async Task<UserLoginResponse> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var accessTokenClaims = await GetLocalClaims(user);

            var expirationAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);

            var accessToken = StrToken(accessTokenClaims, expirationAccessToken);            

            return new UserLoginResponse
            (
                new UserLogin { LoginUserName = email },
                accessToken                
            );
        }

        private async Task<IList<Claim>> GetLocalClaims(IdentityUser user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            return claims;
        }

        private string StrToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var strTokenNew = new JwtSecurityTokenHandler()
                .CreateEncodedJwt(
                    _jwtOptions.Issuer,
                    _jwtOptions.Audience,
                    new ClaimsIdentity(claims),
                    DateTime.Now,
                    dataExpiracao,
                    DateTime.Now,
                    _jwtOptions.SigningCredentials
                );

            return strTokenNew;
        }
    }
}
