using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Orders
{
    public interface IOrderService
    {
        Task PlaceOrder();
        Task<List<OrderViewResponse>> GetOrders();
    }
}
