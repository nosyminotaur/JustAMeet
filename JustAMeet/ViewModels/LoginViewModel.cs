using System.Diagnostics;

namespace JustAMeet.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        /// <summary>
        /// Holds username/email
        /// </summary>
        private string userData;
        //Can the user submit the form?
        private bool isFormValid;

        public string UserData
        {
            get => userData;
            set
            {
                userData = value;
                UpdateData();
                OnPropertyChanged(nameof(UserData));
            }
        }

        public bool IsFormValid
        {
            get => isFormValid;
            set
            {
                isFormValid = value;
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private void UpdateData()
        {
            Debug.WriteLine("Data updated!");
        }
    }
}
