using ECommerce_Blazor.Server.Data;
using ECommerce_Blazor.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Blazor.Server.Service.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }
    }
}
