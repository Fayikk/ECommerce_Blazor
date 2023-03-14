using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Server.Service.ProductTypeService
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<ProductType>>> GetProductTypes();
        Task<ServiceResponse<List<ProductType>>> CreateProductType(ProductType productType);
        Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType);
        Task<ServiceResponse<ProductType>> DeleteProductType(int productTypeId);
    }
}
