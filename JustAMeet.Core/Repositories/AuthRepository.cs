using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using JustAMeet.Core.Interfaces;
using JustAMeet.DAL;
using JustAMeet.Core.Entities;
using Google.Apis.Auth;

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

        /// <summary>
        /// Login using Google. If user exists, returns InternalUser.
        /// If no user found, creates a new AppUser.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InternalUser> GoogleLogin(string token)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);
                var user = await _manager.FindByEmailAsync(payload.Email);
                
                //If user exists, return success
                if (user != null)
                {
                    bool isGoogleLoginProvider = false;
                    //It might be that the user has an account but not using Google as a login provider
                    for (int i = 0; i < user.LoginProviders.Count; i++)
                    {
                        if (user.LoginProviders[i].LoginProviderKey == LoginProvider.Google)
                        {
                            isGoogleLoginProvider = true;
                            break;
                        }
                    }
                    if (!isGoogleLoginProvider)
                    {
                        //Add Google as a Login Provider
                        user.LoginProviders.Add(LoginProvider.GoogleLoginProvider(token));
                        await _manager.UpdateAsync(user);
                    }
                    return new InternalUser { email = user.Email, success = true, username = user.UserName };
                }

                LoginProvider googleloginProvider = LoginProvider.GoogleLoginProvider(token);

                //Create a new user
                user = new AppUser
                {
                    //User part of email before '@' as username. User can change it anytime he likes
                    UserName = payload.Email.Split('@')[0],
                    Email = payload.Email,
                    LoginProviders = new List<LoginProvider> { googleloginProvider }
                };

                var createResult = await _manager.CreateAsync(user);

                if (createResult != IdentityResult.Success)
                {
                    var errors = createResult.Errors.Select(ier => ier.Description);
                    return new InternalUser { success = false, errors = errors.ToList() };
                }

                return new InternalUser(user.UserName, user.Email, true);
            }
            catch (InvalidJwtException)
            {
                return new InternalUser { success = false, errors = new List<string> { "Invalid JWT" } };
            }
            catch(System.Exception)
            {
                return new InternalUser { success = false, errors = new List<string> { "Internal Server Error" } };
            }
        }

        public async Task<bool> UsernameExists(string username)
        {
            var user = await _manager.FindByNameAsync(username);
            return (user != null);
        }

        public async Task<bool> EmailExists(string email)
        {
            var user = await _manager.FindByEmailAsync(email);
            return (user != null);
        }
    }
}