using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp.Models
{
    class CreateOrderRequest
    {
        
        public List<OrderItemRequest> Items { get; set; }
    }

    public class OrderItemRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
