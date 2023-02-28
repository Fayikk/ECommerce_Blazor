
using ECommerce_Blazor.Shared;
using Stripe.Checkout;

namespace ECommerce_Blazor.Server.Service.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request);
    }
}
