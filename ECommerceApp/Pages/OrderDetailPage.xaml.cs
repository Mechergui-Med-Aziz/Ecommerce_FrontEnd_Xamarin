using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ECommerceApp.Models;
namespace ECommerceApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailPage : ContentPage
    {
        public OrderDetailPage(Order order)
        {
            InitializeComponent();
            LoadOrderDetails(order);
        }

        private void LoadOrderDetails(Order order)
        {
            OrderIdLabel.Text = $"Order #{order.Id}";
            OrderDateLabel.Text = $"Date: {order.OrderDate:d}";
            TotalAmountLabel.Text = $"Total: {order.TotalAmount:C}";
            OrderItemsListView.ItemsSource = order.Items;
        }
    }
}