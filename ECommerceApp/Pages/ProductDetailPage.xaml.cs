using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceApp.Models;
using ECommerceApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ECommerceApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Product _product;
        private readonly bool _isNewProduct;

        public event EventHandler ProductAdded;
        public event EventHandler ProductUpdated;

        public ProductDetailPage(Product product, bool isNewProduct)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _product = product;
            _isNewProduct = isNewProduct;

            if (!_isNewProduct)
            {
                NameEntry.Text = _product.name;
                DescriptionEditor.Text = _product.description;
                PriceEntry.Text = _product.price.ToString();
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _product.name = NameEntry.Text;
            _product.description = DescriptionEditor.Text;
            if (decimal.TryParse(PriceEntry.Text, out decimal price))
            {
                _product.price = price;
            }
            else
            {
                await DisplayAlert("Error", "Invalid price", "OK");
                return;
            }

            bool success;
            if (_isNewProduct)
            {
                success = await _apiService.AddProduct(_product.name,_product.description,_product.price.ToString());
                if (success)
                {
                    ProductAdded?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                success = await _apiService.UpdateProduct(_product);
                if (success)
                {
                    ProductUpdated?.Invoke(this, EventArgs.Empty);
                }
            }

            if (success)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to save product", "OK");
            }
        }
    }
}