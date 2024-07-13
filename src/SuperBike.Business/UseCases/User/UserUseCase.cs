using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperBike.Auth.Business;
using SuperBike.Auth.Config;
using SuperBike.Auth.Context;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.Dtos.User;
using SuperBike.Business.Dtos.User.Request;
using SuperBike.Business.Dtos.User.Response;
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

        public async Task<UserInsertResponse> Insert(UserInsertRequest userInsertRequest)
        {

            try
            {
                "Iniciando [insert] de usuário: {LoginUserName}".LogInf(userInsertRequest.Data.LoginUserName);

                var user = userInsertRequest.Data;
                var userInsertResponse = new UserInsertResponse(user) { RequestId = userInsertRequest.RequestId };
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

                        var resultAddUserRole = await UserManager.AddToRoleAsync(identityUser, RoleTypeSuperBike.Deliveryman);

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

                var userInsertResponse = new UserInsertResponse(userInsertRequest.Data) { RequestId = userInsertRequest.RequestId };
                userInsertResponse.Errors.Add(exc.Message);
                return userInsertResponse;
            }

        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest)
        {
            try
            {
                "Iniciando 'login' de usuário: {LoginUserName}".LogInf(userLoginRequest.Data.LoginUserName);

                var user = userLoginRequest.Data;
                var userLoginResponse = new UserLoginResponse(new UserLogin { LoginUserName = user.LoginUserName }) { RequestId = userLoginRequest.RequestId };

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

                var userLoginResponse = new UserLoginResponse(userLoginRequest.Data) { RequestId = userLoginRequest.RequestId };
                userLoginResponse.Errors.Add(exc.Message);                
                return userLoginResponse;
            }
            
        }

        /*
        public override Task UnitOfWorkCommand(Func<Task> command)
        {
            return base.UnitOfWorkCommand(command);
        }
        
        private override async Task UnitOfWorkCommand(Func<Task> command)
        {            
            using var transaction = await AuthIdentityDbContext.Database.BeginTransactionAsync();

            try
            {
                await command();
                await transaction.CommitAsync();                
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        */
        /*
        public override async Task UnitOfWorkExecute(Func<Task> execute)
        {
            using var transaction = await AuthIdentityDbContext.Database.BeginTransactionAsync();

            try
            {
                await execute();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        */
        private async Task<UserLoginResponse> GenerateToken(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var accessTokenClaims = await GetLocalClaims(user ?? new IdentityUser());

            var expirationAccessToken = DateTime.Now.AddSeconds(JwtOptions.AccessTokenExpiration);

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
