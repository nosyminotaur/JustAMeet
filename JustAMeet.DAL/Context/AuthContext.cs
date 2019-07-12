using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JustAMeet.DAL.Context
{
    public class AuthContext : IdentityDbContext<AppUser>
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var LoginTypeToStringConverter = new ValueConverter<List<LoginProvider>, string>(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<List<LoginProvider>>(v));
            builder.Entity<AppUser>().Property(e => e.LoginProviders).HasConversion(LoginTypeToStringConverter);
        }
    }
}
