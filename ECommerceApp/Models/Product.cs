using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp.Models
{
    public class Product
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
    }
}
