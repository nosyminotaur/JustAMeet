using JustAMeet.Shared.Models;
using System.Threading.Tasks;

namespace JustAMeet.Services
{
    public interface IAuthService
    {
        Task<UserOutDTO> UsernameLogin(string username, string password);
        Task<UserOutDTO> EmailLogin(string email, string password);
    }
}
