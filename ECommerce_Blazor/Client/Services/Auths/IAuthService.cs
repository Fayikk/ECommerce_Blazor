using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Auths
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin user);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword changePassword);
        Task<bool> IsUserAuthenticated();
    }
}
