using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceApp.Models;
using ECommerceApp.Services;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ECommerceApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        private readonly ApiService _apiService;
        private List<Product> _products;

        public ProductsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            _products = await _apiService.GetProducts();
            ProductsListView.ItemsSource = _products;
        }

        private async void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Product product)
            {
                var result = await DisplayAlert("Add to Cart", $"Add {product.name} to cart?", "Yes", "No");
                if (result)
                {
                    await _apiService.AddToCart(product.id, 1);
                    await DisplayAlert("Success", "Product added to cart", "OK");
                }
            }
        }

        private async void AddProduct(object sender, EventArgs e) // Corrected signature
        {
            await Navigation.PushAsync(new ProductAdd());
        }




        private async void OnViewCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }
        /*private async void Disconnect(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }*/


        private async void Disconnect(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirmation", "Are you sure you want to log out?", "Yes", "No");
            if (!confirm) return;

            try
            {
                
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                
                await DisplayAlert("Error", $"An error occurred during logout: {ex.Message}", "OK");
            }
        }
    }
}
