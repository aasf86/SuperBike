using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuperBike.Auth.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Auth.Config
{
    public static class AuthConfig
    {
        public static void AddAuthSuperBike(this IHostApplicationBuilder builder)
        {
            var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecurityKey));

            builder.Services.Configure<JwtOptions>(op => 
            {
                op.Issuer = jwtOptions.Issuer;
                op.Audience = jwtOptions.Audience;                
                op.AccessTokenExpiration = jwtOptions.AccessTokenExpiration;
                op.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
                op.SecurityKey = jwtOptions.SecurityKey;
            });

            var identityOptions = builder.Configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>();

            builder.Services.Configure<IdentityOptions>(op => 
            {
                op.Password.RequireDigit = identityOptions?.Password.RequireDigit ?? true;
                op.Password.RequireLowercase = identityOptions?.Password.RequireLowercase ?? true;
                op.Password.RequireNonAlphanumeric = identityOptions?.Password.RequireNonAlphanumeric ?? true;
                op.Password.RequireUppercase = identityOptions?.Password.RequireUppercase ?? true;
                op.Password.RequiredLength = identityOptions?.Password.RequiredLength ?? 6;
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                }; ;
            });

            builder.Services.AddDbContext<AuthIdentityDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default"), o => o.SetPostgresVersion(12, 0))
            );

            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthIdentityDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
