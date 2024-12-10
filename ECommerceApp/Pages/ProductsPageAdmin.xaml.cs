using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceApp.Models;
using ECommerceApp.Services;
using ECommerceApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ECommerceApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPageAdmin : ContentPage
    {
        private readonly ApiService _apiService;

        public ProductsPageAdmin()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            var products = await _apiService.GetProducts();
            ProductsListView.ItemsSource = products;
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            var product = new Product();
            var page = new ProductDetailPage(product, true);
            page.ProductAdded += OnProductAdded;
            await Navigation.PushAsync(page);
        }

        private void OnProductAdded(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private async void OnEditProductClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.CommandParameter;
            var page = new ProductDetailPage(product, false);
            page.ProductUpdated += OnProductUpdated;
            await Navigation.PushAsync(page);
        }

        private void OnProductUpdated(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private async void OnDeleteProductClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.CommandParameter;
            var result = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {product.name}?", "Yes", "No");
            if (result)
            {
                var success = await _apiService.DeleteProduct(product.id);
                if (success)
                {
                    LoadProducts();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete product", "OK");
                }
            }
        }

        private void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
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