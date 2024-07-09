using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuperBike.Auth.Context
{
    public class AuthIdentityDbContext : IdentityDbContext
    {
        public AuthIdentityDbContext(DbContextOptions<AuthIdentityDbContext> options) : base(options) { }
    }
}
