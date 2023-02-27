using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Server.Service.CategoryService
{
    public interface ICategoryService 
    {
        Task<ServiceResponse<List<Category>>> GetCategories();

    }
}
