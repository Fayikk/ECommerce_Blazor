using ECommerce_Blazor.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ECommerce_Blazor.Client.Services.Orders
{
    public class OrderService:IOrderService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient client, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            _client = client;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
        {
            var result = await _client.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/{orderId}");
            return result.Data;
        }

        public async Task<List<OrderViewResponse>> GetOrders()
        {

            var response = await _client.GetFromJsonAsync<ServiceResponse<List<OrderViewResponse>>>("api/order");
            return response.Data;


        }

        public async Task<string> PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
               var result =  await _client.PostAsync("api/payment/checkout", null);
                var url = await result.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                return "login";
            }
            
        }


        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
