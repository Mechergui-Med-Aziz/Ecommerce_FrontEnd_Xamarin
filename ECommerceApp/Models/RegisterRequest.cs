using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApp.Models
{
    class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
