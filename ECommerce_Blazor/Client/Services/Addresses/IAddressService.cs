using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Addresses
{
    public interface IAddressService
    {
        Task<Address> GetAddress();
        Task<Address> AddOrUpdateAddress(Address address);
    }
}
