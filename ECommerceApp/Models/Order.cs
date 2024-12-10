using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp.Models
{
    public class Order
    {
        public long Id { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
