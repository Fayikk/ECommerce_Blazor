using ECommerce_Blazor.Server.Service.ProductTypeService;
using ECommerce_Blazor.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;
        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProducts()
        {
            var result = await _productTypeService.GetProductTypes();
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<ProductType>>> CreateProductType(ProductType productType)
        {
            var result = await _productTypeService.CreateProductType(productType);
            return Ok(result);  
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<ProductType>>> UpdateProductType(ProductType productType)
        {
            var result = await _productTypeService.UpdateProductType(productType);
            return Ok(result);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<ServiceResponse<ProductType>>> DeleteProductType([FromRoute]int Id)
        {
            var result = await _productTypeService.DeleteProductType(Id);
            return Ok(result);  
        }

    }
}
