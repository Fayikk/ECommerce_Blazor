using ECommerce_Blazor.Server.Data;
using ECommerce_Blazor.Server.Service.AuthService;
using ECommerce_Blazor.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Blazor.Server.Service.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        public CartService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        //private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
        {
            var response = new ServiceResponse<List<CartProductResponse>>
            {
                Data = new List<CartProductResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products
                       .Where(p => p.Id == item.Id)
                       .FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _context.ProductVariants
                    .Where(v => v.ProductId == item.Id
                         && v.ProductTypeId == item.ProductTypeId).Include(v => v.ProductType).FirstOrDefaultAsync();
                if (productVariant == null)
                {
                    continue;
                }

                var cartProduct = new CartProductResponse
                {
                    ImageUrl = product.ImageUrl,
                    ProductId = product.Id,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    Title = product.Title,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = item.Quantity
                };
                response.Data.Add(cartProduct);
            }
            return response;

        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItems => cartItems.UserId = _authService.GetUserId());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();
            return await GetDbProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            //öncellikle kullanıcı kontrolü yapılmalı
            var count = (await _context.CartItems.Where(x => x.UserId == _authService.GetUserId()).ToListAsync()).Count;


            var response = new ServiceResponse<int>
            {
                Data = count,
            };
            return response;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbProducts(int? userId=null)
        {
            if (userId == null)
            {
                userId = _authService.GetUserId();
            }
            return await GetCartProducts(await _context.CartItems.Where(a => a.UserId == userId).ToListAsync());

        }



        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();
            var sameItem = await _context.CartItems.FirstOrDefaultAsync(x => x.Id == cartItem.Id && x.ProductTypeId == cartItem.ProductTypeId && x.UserId == cartItem.UserId);

            if (sameItem == null)
            {
                _context.CartItems.Add(cartItem);

            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;

            }
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> ConfigurationQuantity(CartItem cartItem)
        {

            var quantityItem = await _context.CartItems
                .FirstOrDefaultAsync(x => x.Id == cartItem.Id && x.ProductTypeId == cartItem.ProductTypeId && x.UserId == _authService.GetUserId());
            if (quantityItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist",
                };
            }
            quantityItem.Quantity += cartItem.Quantity;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == productId &&
                ci.ProductTypeId == productTypeId && ci.UserId == _authService.GetUserId());
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };
            }

            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
