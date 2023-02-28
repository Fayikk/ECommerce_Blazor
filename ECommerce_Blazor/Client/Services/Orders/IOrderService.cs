using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Orders
{
    public interface IOrderService
    {
        Task<string> PlaceOrder();
        Task<List<OrderViewResponse>> GetOrders();
        Task<OrderDetailsResponse> GetOrderDetails(int orderId);
    }
}
