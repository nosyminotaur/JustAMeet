using System;
using System.Collections.Generic;
using System.Text;

namespace JustAMeet.DAL
{
    public class LoginProvider
    {
        public static readonly string Google = "google";
        public static readonly string Facebook = "facebook";

        public string LoginProviderName;
        public string LoginProviderKey;

        public static LoginProvider GoogleLoginProvider(string token)
        {
            return new LoginProvider { LoginProviderKey = token, LoginProviderName = Google };
        }
    }
}
