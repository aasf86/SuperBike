using Microsoft.IdentityModel.Tokens;

namespace SuperBike.Auth.Config
{
    public class JwtOptions
    {
        public SigningCredentials? SigningCredentials { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? SecurityKey { get; set; }
    }
}
