using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace JustAMeet.DAL
{
    public class AppUser : IdentityUser
    {
        public List<LoginProvider> LoginProviders;
    }
}
