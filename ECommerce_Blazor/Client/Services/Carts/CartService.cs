using Blazored.LocalStorage;
using ECommerce_Blazor.Client.Services.Auths;
using ECommerce_Blazor.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ECommerce_Blazor.Client.Services.Carts
{
    public class CartService:ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthService _authService;
        private readonly HttpClient _http;
        public CartService(ILocalStorageService localStorage, HttpClient http,IAuthService authService)
        {
            _authService= authService;
            _http = http;
            _localStorage = localStorage;
        }

        //public string Message { get; set; } = "Product not fount into the basket";

        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _http.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {

                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");//çnce listele
                if (cart == null)
                {
                    cart = new List<CartItem>();//sonra kontrol et
                }

                var sameItem = cart.Find(x => x.Id == cartItem.Id &&
                    x.ProductTypeId == cartItem.ProductTypeId);//istenilen değerlerin eşleşmesini sağla
                if (sameItem == null)
                {
                    cart.Add(cartItem);//eğer eşleşme yok ise yeni ekle
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;//eşleşme var ise 1 adet arrtır
                }

                await _localStorage.SetItemAsync("cart", cart);//kaydet
            }

            await CartItemQuantity();
        }

        public async Task CartItemQuantity()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/cart-count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }
            OnChange.Invoke();
        }



        public async Task<List<CartProductResponse>> GetCartProduct()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("api/cart");
                return response.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                {
                    return new List<CartProductResponse>();
                }
                var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);

                var cartProducts =
                         await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
                return cartProducts.Data;
            }


        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {

            if (await _authService.IsUserAuthenticated())
            {
                await _http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {
                var deleteCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");//verileri çekti
                if (deleteCart == null)
                {//kontrol
                    return;
                }
                var cartItem = deleteCart.Find(x => x.Id == productId && x.ProductTypeId == productTypeId);//doğru nesne bulundu
                if (cartItem != null)
                {
                    deleteCart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", deleteCart);
                }
            }
            //await CartItemQuantity();


        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }
            await _http.PostAsJsonAsync("api/cart/cart-item", localCart);
            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartProductResponse product)
        {
            if (await _authService.IsUserAuthenticated())
            {
                var request = new CartItem
                {
                    Id = product.ProductId,
                    ProductTypeId = product.ProductTypeId,
                    Quantity = product.Quantity,

                };
                await _http.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.Id == product.ProductId
                    && x.ProductTypeId == product.ProductTypeId);
                if (cartItem != null)
                {
                    cartItem.Quantity = product.Quantity;
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }


        }

       

    }
}
