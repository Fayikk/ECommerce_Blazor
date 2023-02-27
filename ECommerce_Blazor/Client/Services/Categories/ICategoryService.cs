using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Categories
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }
        Task GetCategories();
    }
}
