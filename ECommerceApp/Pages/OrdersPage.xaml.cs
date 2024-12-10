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
    public partial class OrdersPage : ContentPage
    {
        private readonly ApiService _apiService;

        public OrdersPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadOrders();
        }

        private async void LoadOrders()
        {
            var orders = await _apiService.GetOrders();
            OrdersListView.ItemsSource = orders;
        }
    }
}