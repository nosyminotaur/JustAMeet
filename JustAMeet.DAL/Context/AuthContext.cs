using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JustAMeet.DAL.Context
{
    public class AuthContext : IdentityDbContext<AppUser>
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
    }
}
