using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using JustAMeet.Core.Interfaces;
using JustAMeet.DAL;
using JustAMeet.Core.Entities;

namespace JustAMeet.Core.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _manager;
        public AuthRepository(UserManager<AppUser> manager)
        {
            _manager = manager;
        }

        /* if no such email exists, return error email not found.
        if email exists and password is correct, return user.
        if email exists and password is incorrect, return error password incorrect.
        */
        public async Task<InternalUser> EmailLogin(string email, string password)
        {
            var result = await _manager.FindByEmailAsync(email);
            if (result == null)
            {
                return new InternalUser
                {
                    success = false,
                    errors = new List<string>() {"Email not found"}
                };
            }

            bool isPasswordCorrect = await _manager.CheckPasswordAsync(result, password);
            if (isPasswordCorrect)
            {
                return new InternalUser(result.UserName, result.Email, true);
            }
            return new InternalUser
            {
                success = false,
                errors = new List<string>() {"Incorrect password"}
            };
        }

        /* similar to emaillogin, except first call of getting user from username instead of email
         */
        public async Task<InternalUser> UsernameLogin(string username, string password)
        {
            var result = await _manager.FindByNameAsync(username);

            if (result == null)
            {
                return new InternalUser() 
                {
                    success = false,
                    errors = new List<string>() {"Email not found"}
                };
            }

            bool isPasswordCorrect = await _manager.CheckPasswordAsync(result, password);
            if (isPasswordCorrect)
            {
                return new InternalUser(result.UserName, result.Email, true);
            }
            return new InternalUser() 
            {
                success = false,
                errors = new List<string>() {"Incorrect password"}
            };
        }

        public async Task<InternalUser> Register(string email, string username, string password)
        {
            var result = await _manager.FindByNameAsync(username);
            
            //If username exists already, return error
            if (result != null)
            {
                return new InternalUser { success = false, errors = new List<string> { "Username exists" } };
            }

            //If email exists already, return error
            result = await _manager.FindByEmailAsync(email);
            if (result != null)
            {
                return new InternalUser { success = false, errors = new List<string> { "Email exists" } };
            }

            AppUser user = new AppUser
            {
                UserName = username,
                Email = email,
                Id = System.Guid.NewGuid().ToString()
            };

            var createResult = await _manager.CreateAsync(user, password);
            
            if (createResult != IdentityResult.Success)
            {
                var errors = createResult.Errors.Select(ier => ier.Description);
                return new InternalUser { success = false, errors = errors.ToList() };
            }

            return new InternalUser(username, email, true);
        }
    }
}