using System.Threading.Tasks;
using JustAMeet.Core.Entities;

namespace JustAMeet.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<InternalUser> EmailLogin(string email, string password);
        Task<InternalUser> UsernameLogin(string email, string password);
        Task<InternalUser> Register(string email, string username, string password);
    }
}