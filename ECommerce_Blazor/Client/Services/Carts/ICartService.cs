using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Carts
{
    public interface ICartService
    {
        event Action OnChange;
        //public string Message { get; set; } 
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductResponse>> GetCartProduct();
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductResponse product);
        Task StoreCartItems(bool emptyLocalCart);

        Task CartItemQuantity();
    }
}
