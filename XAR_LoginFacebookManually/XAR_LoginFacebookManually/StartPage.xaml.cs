using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XAR_LoginFacebookManually.Pages;

namespace XAR_LoginFacebookManually
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();

            //Register Events
            btnLogin.Clicked += OnLoginClicked;
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoginFacebookPage());
        }
    }
}
