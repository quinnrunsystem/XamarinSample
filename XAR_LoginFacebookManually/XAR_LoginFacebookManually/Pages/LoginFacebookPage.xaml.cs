using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XAR_LoginFacebookManually.Services;

namespace XAR_LoginFacebookManually.Pages
{
    public partial class LoginFacebookPage : ContentPage
    {
        public LoginFacebookPage()
        {
            InitializeComponent();

            webViewLogin.Source = FacebookServices.Url_Login;
            webViewLogin.Navigated += OnNaviagedWebView;
        }
        private void OnNaviagedWebView(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.Contains(FacebookServices.Redirect_Url))
                if(FacebookServices.UpdateToken(e.Url))
                {
                    Navigation.PushModalAsync(new MainPage());
                }
        }
    }
}
