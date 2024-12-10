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
    public partial class CartPage : ContentPage
    {
        private readonly ApiService _apiService;
        private List<CartItem> _cartItems;

        public CartPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadCart();
            _cartItems = new List<CartItem>();
        }

        private async void LoadCart()
        {
            _cartItems = await _apiService.GetCart();
            CartItemsListView.ItemsSource = _cartItems;
        }

        private void UpdateTotalPrice()
        {
            if (_cartItems != null && _cartItems.Any())
            {
                var total = _cartItems.Sum(item => item.Product.price * item.Quantity);
                TotalPriceLabel.Text = $"Total: {total:C}";
            }
            else
            {
                TotalPriceLabel.Text = "Total: $0.00";
            }
        }


        private async void OnIncrementClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItem)button.CommandParameter;

            try
            {
                var updatedItem = await _apiService.UpdateCartItemQuantity(cartItem.Product.id, cartItem.Quantity + 1);
                if (updatedItem != null)
                {
                    var index = _cartItems.IndexOf(cartItem);
                    _cartItems[index] = updatedItem;
                    CartItemsListView.ItemsSource = null;
                    CartItemsListView.ItemsSource = _cartItems;
                    UpdateTotalPrice();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to update quantity", "OK");
                Console.WriteLine($"Error updating quantity: {ex.Message}");
            }
        }

        private async void OnDecrementClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItem)button.CommandParameter;

            if (cartItem.Quantity <= 1)
            {
                var shouldRemove = await DisplayAlert("Remove Item",
                    "Do you want to remove this item from your cart?",
                    "Yes", "No");

                if (shouldRemove)
                {
                    await RemoveCartItem(cartItem);
                }
                return;
            }

            try
            {
                var updatedItem = await _apiService.UpdateCartItemQuantity(cartItem.Product.id, cartItem.Quantity - 1);
                if (updatedItem != null)
                {
                    var index = _cartItems.IndexOf(cartItem);
                    _cartItems[index] = updatedItem;
                    CartItemsListView.ItemsSource = null;
                    CartItemsListView.ItemsSource = _cartItems;
                    UpdateTotalPrice();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to update quantity", "OK");
                Console.WriteLine($"Error updating quantity: {ex.Message}");
            }
        }

        private async void OnRemoveClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItem)button.CommandParameter;
            await RemoveCartItem(cartItem);
        }

        private async Task RemoveCartItem(CartItem cartItem)
        {
            try
            {
                // Assurez-vous d'utiliser l'ID du produit correct
                var productId = cartItem.Product?.id ?? cartItem.ProductId;
                Console.WriteLine($"Removing product with ID: {productId}");

                var result = await _apiService.RemoveFromCart(productId);
                if (result)
                {
                    _cartItems.Remove(cartItem);
                    CartItemsListView.ItemsSource = null;
                    CartItemsListView.ItemsSource = _cartItems;
                    UpdateTotalPrice();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to remove item from cart", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to remove item from cart", "OK");
                Console.WriteLine($"Error removing item: {ex.Message}");
            }
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            if (_cartItems == null || !_cartItems.Any())
            {
                await DisplayAlert("Error", "Your cart is empty. Please add items before checking out.", "OK");
                return;
            }

            var order = await _apiService.ClearCart();

            if (order != null)
            {
                await DisplayAlert("Success", "Order Passed Successfully", "OK");
                await Navigation.PushAsync(new ProductsPage());
            }
            else
            {
                await DisplayAlert("Error", "Failed to passe order. Please try again.", "OK");
            }
        }

    }
}