using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.ProductTypes
{
    public interface IProductTypeService
    {
        event Action OnChange;
        public List<ProductType> ProductTypes {get; set;}
        Task  GetProductTypes();
        Task AddProductType(ProductType productType);
        Task UpdateProductType(ProductType productType);
        Task DeleteProductType(int productTypeId);
        ProductType CreateNewProductType();
    }
}
