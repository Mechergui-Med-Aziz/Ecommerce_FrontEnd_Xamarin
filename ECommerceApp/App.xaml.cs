using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ECommerceApp.Pages;
using Xamarin.Essentials;

namespace ECommerceApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SplashPage());
        }

        protected override async void OnStart()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                MainPage = new NavigationPage(new ProductsPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
