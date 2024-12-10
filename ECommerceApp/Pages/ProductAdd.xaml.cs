using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ECommerceApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductAdd : ContentPage
    {
        private readonly ApiService _apiService;
        public ProductAdd()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var name1 = name.Text;
            var description1 = description.Text;
            var price1 = price.Text;

            if (string.IsNullOrEmpty(name1) || string.IsNullOrEmpty(description1) || string.IsNullOrEmpty(price1))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            var isSuccess = await _apiService.AddProduct(name1, description1, price1);

            if (isSuccess)
            {
                await DisplayAlert("Success", "Product added succesfully.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Registration failed. Please try again.", "OK");
            }
        }
    }
}