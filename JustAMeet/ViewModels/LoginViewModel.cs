using JustAMeet.Services;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace JustAMeet.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; private set; }
        private IAuthService authService;

        public LoginViewModel()
        {
            LoginCommand = new Command(Login);
            authService = new AuthService();
        }

        public void Login()
        {
            //Define username/email properties and then call the authService.Login() method
        }
    }
}
