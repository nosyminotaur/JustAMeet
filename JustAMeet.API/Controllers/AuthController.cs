using System;
using System.Threading.Tasks;
using JustAMeet.API.DTO;
using JustAMeet.Core.Entities;
using JustAMeet.Core.Interfaces;
using JustAMeet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JustAMeet.API.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region private variables
        //keep it empty right now, add authrepo later on
        private readonly IConfiguration configuration;
        private readonly IAuthRepository authRepo;
        private readonly string SECRET_KEY;
        //TODO Use appsettings instead for EXPIRE_TIME
        private readonly double EXPIRE_TIME = 300;
        private ILogger<AuthController> logger;
        #endregion
        public AuthController(IConfiguration configuration, IAuthRepository authRepo, ILogger<AuthController> logger)
        {
            this.configuration = configuration;
            this.authRepo = authRepo;
            this.logger = logger;
            SECRET_KEY = this.configuration.GetSection("JWTKey").Value;
        }

        //Allow access to anyone for creating a new account
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody]SignupDTO userIn)
        {
            logger.LogInformation("Username: " + userIn.username);
            logger.LogInformation("Email: " + userIn.email);
            logger.LogInformation(this.Request.ContentLength.ToString());
            InternalUser result = await authRepo.Register(userIn.email, userIn.username, userIn.password);
            if (result.success)
            {
                string token = JWTHelper.GenerateToken(result.email, result.username, SECRET_KEY, EXPIRE_TIME);
                return Ok(new UserOutDTO
                {
                    Email = result.email,
                    Username = result.username,
                    JwtToken = token,
                    success = true
                });
            }
            else
            {
                //No need to create a new UserOutDTO object, InternalUser doesn't contain any sensitive info
                return BadRequest(result);
            }
        }

        [HttpPost("email-login")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailLogin([FromBody]EmailLoginDTO userIn)
        {
            InternalUser result = await authRepo.EmailLogin(userIn.email, userIn.password);
            if (!result.success)
            {
                return BadRequest(result);
            }
            string token = JWTHelper.GenerateToken(result.email, result.username, SECRET_KEY, EXPIRE_TIME);
            return Ok(new UserOutDTO
            {
                Username = result.username,
                Email = result.email,
                JwtToken = token,
                success = true
            });
        }

        [AllowAnonymous]
        [HttpPost("username-login")]
        public async Task<IActionResult> UsernameLogin([FromBody]UsernameLoginDTO userIn)
        {
            InternalUser result = await authRepo.UsernameLogin(userIn.username, userIn.password);
            if (!result.success)
            {
                return BadRequest(result);
            }
            string token = JWTHelper.GenerateToken(result.email, result.username, SECRET_KEY, EXPIRE_TIME);
            return Ok(new UserOutDTO
            {
                Username = result.username,
                Email = result.email,
                JwtToken = token,
                success = true
            });
        }

        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody]string token)
        {
            InternalUser result = await authRepo.GoogleLogin(token);
            if (!result.success)
            {
                return BadRequest(result);
            }
            string jwt = JWTHelper.GenerateToken(result.email, result.username, SECRET_KEY, EXPIRE_TIME);
            return Ok(new UserOutDTO
            {
                Username = result.username,
                Email = result.email,
                JwtToken = jwt,
                success = true
            });
        }

        [AllowAnonymous]
        [HttpGet("username-exists")]
        public async Task<IActionResult> UsernameExists([FromBody]string username)
        {
            var userexists = await authRepo.UsernameExists(username);
            return Ok(new
            {
                userexists
            });
        }

        [AllowAnonymous]
        [HttpGet("email-exists")]
        public async Task<IActionResult> EmailExists([FromBody]string email)
        {
            var userexists = await authRepo.UsernameExists(email);
            return Ok(new
            {
                userexists
            });
        }
    }
}