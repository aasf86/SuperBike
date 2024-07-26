using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperBike.Auth.Business;
using SuperBike.Auth.Config;
using SuperBike.Auth.Context;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.Dtos;
using SuperBike.Business.Dtos.User;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SuperBike.Business.UseCases.User
{
    public class UserUseCase : UseCaseBase, IUserUseCase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;        
        private readonly JwtOptions _jwtOptions;
        private readonly AuthIdentityDbContext _authIdentityDbContext;

        private SignInManager<IdentityUser> SignInManager => _signInManager;
        private UserManager<IdentityUser> UserManager => _userManager;
        private RoleManager<IdentityRole> RoleManager => _roleManager;
        private JwtOptions JwtOptions => _jwtOptions;
        private AuthIdentityDbContext AuthIdentityDbContext => _authIdentityDbContext;

        public UserUseCase(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtOptions> jwtOptions,
            AuthIdentityDbContext authIdentityDbContext,
            ILogger<UserUseCase> logger) : base(logger, authIdentityDbContext.Database.GetDbConnection())
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _roleManager = roleManager;
            _authIdentityDbContext = authIdentityDbContext;
            TransactionAssigner.Add(transaction => authIdentityDbContext.Database.UseTransaction(transaction as DbTransaction));
        }

        public async Task<ResponseBase<UserInsert>> Insert(RequestBase<UserInsert> userInsertRequest)
        {

            try
            {
                "Iniciando [insert] de usuário: {LoginUserName}".LogInf(userInsertRequest.Data.LoginUserName);

                var user = userInsertRequest.Data;
                var userInsertResponse = ResponseBase.New(user, userInsertRequest.RequestId);
                var result = Validate(user);

                if (!result.IsSuccess)
                {
                    userInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", userInsertResponse.Errors.ToArray());
                    $"Usuário inválido '{{LoginUserName}}': {errors} ".LogWrn(user.LoginUserName);
                    return userInsertResponse;
                }

                var identityUser = new IdentityUser
                {
                    UserName = user.LoginUserName,
                    Email = user.LoginUserName,
                    EmailConfirmed = true
                };

                await UnitOfWorkExecute(async () =>
                {
                    var resultCreate = await UserManager.CreateAsync(identityUser, user.Password);

                    if (resultCreate.Succeeded)
                    {
                        await UserManager.SetLockoutEnabledAsync(identityUser, false);

                        var resultAddUserRole = await UserManager.AddToRoleAsync(identityUser, RoleTypeSuperBike.RenterDeliveryman);

                        if (!resultAddUserRole.Succeeded)
                        {
                            userInsertResponse.Errors.AddRange(resultAddUserRole.Errors.Select(r => r.Description));
                            var errors = string.Join("\n", userInsertResponse.Errors.ToArray());                            
                            $"Validações do Identity {{LoginUserName}} Erros: {errors}".LogWrn(user.LoginUserName);
                        }
                    }
                    else
                    {
                        userInsertResponse.Errors.AddRange(resultCreate.Errors.Select(r => r.Description));
                        var errors = string.Join("\n", userInsertResponse.Errors.ToArray());
                        $"Validações do Identity {{LoginUserName}} Erros: {errors}".LogWrn(user.LoginUserName);
                    }
                });

                return userInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [inserir] usuário: {LoginUserName}".LogErr(userInsertRequest.Data.LoginUserName);
                exc.Message.LogErr(exc);

                var userInsertResponse = ResponseBase.New(userInsertRequest.Data, userInsertRequest.RequestId);
#if DEBUG
                userInsertResponse.Errors.Add(exc.Message);
#endif
                userInsertResponse.Errors.Add("Erro ao inserir usuário.");

                return userInsertResponse;
            }

        }

        public async Task<ResponseBase<UserLogin>> Login(RequestBase<UserLogin> userLoginRequest)
        {
            try
            {
                "Iniciando 'login' de usuário: {LoginUserName}".LogInf(userLoginRequest.Data.LoginUserName);

                var user = userLoginRequest.Data;
                var userLoginResponse = ResponseBase.New(new UserLogin(user.LoginUserName, null), userLoginRequest.RequestId);

                var result = await SignInManager.PasswordSignInAsync(user.LoginUserName, user.Password, false, true);
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

                var userLoginResponse = ResponseBase.New(userLoginRequest.Data, userLoginRequest.RequestId);
#if DEBUG
                userLoginResponse.Errors.Add(exc.Message);
#endif
                userLoginResponse.Errors.Add("Erro login usuário.");

                return userLoginResponse;
            }
            
        }

        private async Task<ResponseBase<UserLogin>> GenerateToken(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var accessTokenClaims = await GetLocalClaims(user ?? new IdentityUser());

            var expirationAccessToken = DateTime.Now.AddSeconds(JwtOptions.AccessTokenExpiration);

            var accessToken = StrToken(accessTokenClaims, expirationAccessToken);

            return ResponseBase.New(new UserLogin(email, accessToken), Guid.Empty);
        }

        private async Task<IList<Claim>> GetLocalClaims(IdentityUser user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, (user.Email ?? "")));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            var userClaims = await UserManager.GetClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            return claims;
        }

        private string StrToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var strTokenNew = new JwtSecurityTokenHandler()
                .CreateEncodedJwt(
                    JwtOptions.Issuer,
                    JwtOptions.Audience,
                    new ClaimsIdentity(claims),
                    DateTime.Now,
                    dataExpiracao,
                    DateTime.Now,
                    JwtOptions.SigningCredentials
                );

            return strTokenNew;
        }
    }
}
