using Blazored.LocalStorage;
using ECommerce_Blazor.Client;
using ECommerce_Blazor.Client.Services.Auths;
using ECommerce_Blazor.Client.Services.Carts;
using ECommerce_Blazor.Client.Services.Categories;
using ECommerce_Blazor.Client.Services.Orders;
using ECommerce_Blazor.Client.Services.Products;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICartService,CartService>(); 
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();



await builder.Build().RunAsync();
