using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JustAMeet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutMe : ContentPage
    {
        public AboutMe()
        {
            InitializeComponent();
        }

        private void StackLayout_LayoutChanged(object sender, EventArgs e)
        {

        }

        private void AboveEighteenSwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }
        //private void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        //{
        //    HeaderLabel.Text = args.NewValue.ToString("F3");
        //}
    }
}