using System.Collections.Generic;

namespace JustAMeet.Core.Entities
{
    //Stores username and email when success is true
    //Does not store anything if success is false
    public class InternalUser
    {
        public InternalUser() {}
        public InternalUser(string username, string email, bool success)
        {
            this.username = username;
            this.email = email;
            this.success = success;
        }

        public string username;
        public string email;
        public List<string> errors;
        public bool success;
    }
}