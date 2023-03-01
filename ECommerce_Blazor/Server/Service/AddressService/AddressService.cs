using ECommerce_Blazor.Server.Data;
using ECommerce_Blazor.Server.Service.AuthService;
using ECommerce_Blazor.Server.Service.OrderService;
using ECommerce_Blazor.Shared;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Blazor.Server.Service.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;
        private readonly DataContext _context;
        public AddressService(DataContext context,IOrderService orderService,IAuthService authService)
        {
            _context = context;
            _orderService = orderService;
            _authService = authService;
        }

        public async Task<ServiceResponse<Address>> AddUpdateAddress(Address address)
        {
            var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddress()).Data;
            if (dbAddress == null)
            {
                address.UserId = _authService.GetUserId();
                _context.Addresses.Add(address);
                response.Data = address;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.State = address.State;
                dbAddress.Country = address.Country;
                dbAddress.City = address.City;
                dbAddress.Zip = address.Zip;
                dbAddress.Street = address.Street;
                response.Data = dbAddress;
            }

            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = _authService.GetUserId();
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);
            return new ServiceResponse<Address> { Data = address };
        }
    }
}
