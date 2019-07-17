using System.ComponentModel.DataAnnotations;

namespace JustAMeet.Shared.Models
{
    public class UsernameLoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string email;

        [Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username must be 5 characters long")]
        //TODO Add regex to control username characters
        public string username;

        [Required(ErrorMessage = "Password is required")]
        //TODO Add regex
        public string password;
    }
}
