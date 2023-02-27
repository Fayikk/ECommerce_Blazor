using ECommerce_Blazor.Shared;
using System.Net.Http.Json;

namespace ECommerce_Blazor.Client.Services.Auths
{
    public class AuthService:IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/change-password", request.Password);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<string>> Login(UserLogin user)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();

        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
