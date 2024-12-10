using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp.Models
{
    public class CartItem
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
