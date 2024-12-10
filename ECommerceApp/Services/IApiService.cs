using System.Collections.Generic;
using ECommerceApp.Models;
using System.Threading.Tasks;
using Refit;

namespace ECommerceApp.Services
{
    interface IApiService
    {
        [Post("/api/auth/login")]
        Task<ApiResponse<string>> Login([Body] LoginRequest request);

        [Post("/api/auth/register")]
        Task<ApiResponse<string>> Register([Body] RegisterRequest request);

        [Post("/api/products")]
        Task<ApiResponse<string>> AddProduct([Body] AddProductRequest request);

        [Put("/api/products/{id}")]
        Task<ApiResponse<Product>> UpdateProduct(long id, [Body] Product product);

        [Delete("/api/products/{id}")]
        Task<ApiResponse<bool>> DeleteProduct(long id);

        [Get("/api/products")]
        Task<ApiResponse<List<Product>>> GetProducts();

        [Post("/api/cart/add")]
        Task<ApiResponse<CartItem>> AddToCart([Body] AddToCartRequest request);

        [Get("/api/cart")]
        Task<ApiResponse<List<CartItem>>> GetCart();

        [Put("/api/cart/update/{productId}")]
        Task<ApiResponse<CartItem>> UpdateCartItemQuantity(long productId, [Body] UpdateCartItemRequest request);

        [Delete("/api/cart/remove/{productId}")]
        Task<ApiResponse<bool>> RemoveFromCart(long productId);

        [Delete("/api/cart/clear")]
        Task<ApiResponse<bool>> ClearCart();

        [Post("/api/orders")]
        Task<ApiResponse<Order>> CreateOrder([Body] CreateOrderRequest request);

        [Get("/api/orders")]
        Task<ApiResponse<List<Order>>> GetOrders();

        [Get("/api/user/profile")]
        Task<ApiResponse<User>> GetUserProfile();

       
    }
}
