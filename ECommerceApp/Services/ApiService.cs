using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ECommerceApp.Models;
using Refit;
using Newtonsoft.Json;
using Xamarin.Essentials;
namespace ECommerceApp.Services
{
    public class ApiService
    {
        private readonly IApiService _apiService;
        private const string BaseUrl = "http://172.16.16.84:8080";
        private const string TokenKey = "auth_token";

        public ApiService()
        {
            var httpClient = new HttpClient(new AuthenticatedHttpClientHandler(GetToken))
            {
                BaseAddress = new Uri(BaseUrl)
            };

            _apiService = RestService.For<IApiService>(httpClient);
        }


        private async Task<string> GetToken()
        {
            var tokenJson = await SecureStorage.GetAsync(TokenKey);
            if (!string.IsNullOrEmpty(tokenJson))
            {
                var tokenObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenJson);
                if (tokenObject.TryGetValue("token", out string token))
                {
                    return token;
                }
            }
            return null;
        }

       


        public async Task<LoginResult> Login(string username, string password)
        {
            var request = new LoginRequest { Username = username, Password = password };
            try
            {
                var response = await _apiService.Login(request);
                if (response.IsSuccessStatusCode)
                {
                    await SecureStorage.SetAsync(TokenKey, response.Content);
                    Console.WriteLine("Token stocké avec succès");

                    // Check if it's an admin login
                    if (username.ToLower() == "admin" && password == "admin")
                    {
                        return new LoginResult { IsSuccess = true, IsAdmin = true };
                    }

                    return new LoginResult { IsSuccess = true, IsAdmin = false };
                }
                Console.WriteLine($"Échec de la connexion. Code : {response.StatusCode}");
                return new LoginResult { IsSuccess = false, IsAdmin = false };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la connexion : {ex.Message}");
                return new LoginResult { IsSuccess = false, IsAdmin = false };
            }
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                var response = await _apiService.UpdateProduct(product.id, product);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la mise à jour du produit : {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            try
            {
                var response = await _apiService.DeleteProduct(productId);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la suppression du produit : {ex.Message}");
                return false;
            }
        }

        public class LoginResult
        {
            public bool IsSuccess { get; set; }
            public bool IsAdmin { get; set; }
        }



        public async Task<bool> AddProduct(string name, string description, string price)
        {
            var request = new AddProductRequest { name = name, description = description, price = float.Parse(price) };
            var response = await _apiService.AddProduct(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Register(string username, string email, string password)
        {
            var request = new RegisterRequest { Username = username, Email = email, Password = password };
            var response = await _apiService.Register(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                var response = await _apiService.GetProducts();
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Produits récupérés avec succès. Nombre de produits : {response.Content.Count}");
                    return response.Content;
                }
                else
                {
                    Console.WriteLine($"Erreur lors de la récupération des produits. Code : {response.StatusCode}, Contenu : {response.Error}");
                    return new List<Product>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la récupération des produits : {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task<CartItem> AddToCart(long productId, int quantity)
        {
            var request = new AddToCartRequest { ProductId = productId, Quantity = quantity };
            var response = await _apiService.AddToCart(request);
            return response.IsSuccessStatusCode ? response.Content : null;
        }

        public async Task<List<CartItem>> GetCart()
        {
            var response = await _apiService.GetCart();
            return response.IsSuccessStatusCode ? response.Content : new List<CartItem>();
        }

        public async Task<CartItem> UpdateCartItemQuantity(long productId, int newQuantity)
        {
            var request = new UpdateCartItemRequest { NewQuantity = newQuantity };
            var response = await _apiService.UpdateCartItemQuantity(productId, request);
            return response.IsSuccessStatusCode ? response.Content : null;
        }

        public async Task<bool> RemoveFromCart(long productId)
        {
            var response = await _apiService.RemoveFromCart(productId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ClearCart()
        {
            var response = await _apiService.ClearCart();
            return response.IsSuccessStatusCode;
        }

        public async Task<Order> CreateOrder(List<CartItem> items)
        {
            var orderItems = items.ConvertAll(item => new OrderItemRequest { ProductId = item.Product.id, Quantity = item.Quantity });
            var request = new CreateOrderRequest { Items = orderItems };
            var response = await _apiService.CreateOrder(request);
            return response.IsSuccessStatusCode ? response.Content : null;
        }

        public async Task<List<Order>> GetOrders()
        {
            var response = await _apiService.GetOrders();
            return response.IsSuccessStatusCode ? response.Content : new List<Order>();
        }

        public async Task<User> GetUserProfile()
        {
            var response = await _apiService.GetUserProfile();
            return response.IsSuccessStatusCode ? response.Content : null;
        }
    }

    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly Func<Task<string>> _getToken;

        public AuthenticatedHttpClientHandler(Func<Task<string>> getToken)
        {
            _getToken = getToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _getToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"Token ajouté à la requête: {token}");
            }
            else
            {
                Console.WriteLine("Aucun token trouvé pour la requête");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}