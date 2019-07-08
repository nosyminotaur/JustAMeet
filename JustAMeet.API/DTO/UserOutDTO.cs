using System.Collections.Generic;

namespace JustAMeet.API.DTO
{
    //Object returned to user
    public class UserOutDTO
    {
        public string Username;
        public string Email;
        public string JwtToken;
        public bool success;
        public List<string> errors;
    }
}
