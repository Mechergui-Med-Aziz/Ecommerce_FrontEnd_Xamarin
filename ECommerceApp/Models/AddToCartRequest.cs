using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp.Models
{
    class AddToCartRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
