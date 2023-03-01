using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Categories
{
    public interface ICategoryService
    {
        event Action OnChange;
        List<Category> Categories { get; set; }
        List<Category> AdminCategories { get; set; }
        Task GetCategories();
        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
        Category CreateNewCategory();
    }
}
