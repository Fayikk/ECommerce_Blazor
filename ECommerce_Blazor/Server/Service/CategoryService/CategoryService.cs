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

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
            Category category = await GetByCategoryId(id);
            if (category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Element not found"
                };
            }

            category.Deleted = true;
            await _context.SaveChangesAsync();
            return await GetAdminCategories();


        }



        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(x => !x.Deleted && x.Visible)
                .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var dbCategory = await GetByCategoryId(category.Id);
            if (dbCategory == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Message = "Category Not Found",
                    Success = false,
                };
            }
            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            var categories = await _context.Categories
               .Where(x => !x.Deleted)
               .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        protected async Task<Category> GetByCategoryId(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }
    }




}


