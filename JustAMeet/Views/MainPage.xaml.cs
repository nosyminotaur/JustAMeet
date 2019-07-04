using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace JustAMeet.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Add a fadeanimation to the entire page on load
            MainGrid.FadeTo(1, 2000, Easing.SpringIn);
        }

        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            //Change page to the Login Page
            await Navigation.PushAsync(new LoginPage());
        }
    }
}