using ECommerce_Blazor.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Blazor.Server.Service.AddressService
{
    public interface IAddressService
    {
        Task<ServiceResponse<Address>> GetAddress();
        Task<ServiceResponse<Address>> AddUpdateAddress(Address address);

       
    }
}
