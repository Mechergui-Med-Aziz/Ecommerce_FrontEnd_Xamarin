﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp.Models
{
    class AddProductRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public float price { get; set; }
    }
}
