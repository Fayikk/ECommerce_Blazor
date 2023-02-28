using ECommerce_Blazor.Server.Service.CartService;
using ECommerce_Blazor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProduct([FromBody] List<CartItem> item)
        {
            var result = await _cartService.GetCartProducts(item);
            return Ok(result);
        }

        [HttpPost("cart-item")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItem([FromBody] List<CartItem> item)
        {
            var result = await _cartService.StoreCartItems(item);
            return Ok(result);
        }

        [HttpGet("cart-count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            return await _cartService.GetCartItemsCount();

        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartDbItems()
        {
            var result = await _cartService.GetDbProducts();
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
        {
            var result =await _cartService.AddToCart(cartItem);
            return Ok(result);
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantityCart(CartItem cartItem)
        {
            var result = await _cartService.ConfigurationQuantity(cartItem);
            return Ok(result);
        }

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var result = await _cartService.RemoveItemFromCart(productId, productTypeId);
            return Ok(result);
        }
    }
}
