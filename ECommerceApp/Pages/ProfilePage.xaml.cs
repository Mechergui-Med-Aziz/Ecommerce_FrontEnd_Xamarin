using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ECommerceApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private readonly ApiService _apiService;

        public ProfilePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadUserProfile();
        }

        private async void LoadUserProfile()
        {
            var user = await _apiService.GetUserProfile();
            if (user != null)
            {
                UsernameLabel.Text = user.Username;
                EmailLabel.Text = user.Email;
            }
        }

        private async void OnViewOrdersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdersPage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await SecureStorage.SetAsync("auth_token", string.Empty);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}