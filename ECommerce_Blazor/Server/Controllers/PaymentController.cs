using ECommerce_Blazor.Server.Service.PaymentService;
using ECommerce_Blazor.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace ECommerce_Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;   
        }

        [HttpPost("checkout"),Authorize]
        public async Task<ActionResult<string>> CreateCheckout()
        {
            var result = await _paymentService.CreateCheckoutSession();
            return Ok(result.Url);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> FulFillOrder()
        {
            //For webhook
            var result = await _paymentService.FullfillOrder(Request);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

    }
}
