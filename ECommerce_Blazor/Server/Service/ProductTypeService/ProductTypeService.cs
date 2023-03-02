using ECommerce_Blazor.Server.Data;
using ECommerce_Blazor.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Blazor.Server.Service.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;
        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> CreateProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var result = await _context.ProductTypes.ToListAsync();

            var response = new ServiceResponse<List<ProductType>>
            {
                Data = result,
            };
            return response;
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbCategory = await GetProductTypeId(productType.Id);
            if (dbCategory == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Message = "ProductType is not found",
                    Success=false,
                };
            }
            dbCategory.Name= productType.Name;  
            await _context.SaveChangesAsync();
          return  await GetProductTypes();
        }

        protected async Task<ProductType> GetProductTypeId(int id)
        {
            return await _context.ProductTypes.FindAsync(id);
        }
    }
}
