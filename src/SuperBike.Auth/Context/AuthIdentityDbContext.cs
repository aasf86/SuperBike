using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBike.Auth.Context
{
    public class AuthIdentityDbContext : IdentityDbContext
    {
        public AuthIdentityDbContext(DbContextOptions<AuthIdentityDbContext> options) : base(options) { }
    }
}
