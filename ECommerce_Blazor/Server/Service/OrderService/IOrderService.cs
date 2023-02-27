using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Server.Service.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        Task<ServiceResponse<List<OrderViewResponse>>> GetOrders();
    }
}
