using ECommerce_Blazor.Server.Data;
using ECommerce_Blazor.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Blazor.Server.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProductService(DataContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Visible && !p.Deleted).Include(p => p.Variants.Where(p => p.Visible && !p.Deleted)).ToListAsync(),
            };
            return response;

        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            Product product = null;

            if (_contextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _context.Products
                .Include(x => x.Variants.Where(v => !v.Deleted))
                .ThenInclude(t => t.ProductType)
                .Include(p=>p.Images)
                .FirstOrDefaultAsync(a => a.Id == productId && !a.Deleted);
            }
            else
            {
                product = await _context.Products
                .Include(x => x.Variants.Where(v => v.Visible && !v.Deleted))
                .ThenInclude(t => t.ProductType)
                .FirstOrDefaultAsync(a => a.Id == productId && !a.Deleted && a.Visible);
            }


            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(x => x.Category.Url.ToLower().Equals(categoryUrl.ToLower()) && x.Visible && !x.Deleted)
                    .Include(p => p.Variants.Where(p => p.Visible && !p.Deleted))
                    .ToListAsync()

            };
            return response;
        }

        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var pageResults = 3f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / pageResults);
            var products = await _context.Products
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                                ||
                                p.Description.ToLower().Contains(searchText.ToLower())
                                && p.Visible && !p.Deleted
                                )
                                .Include(p => p.Variants).Include(p=>p.Images).Where(p => p.Visible && !p.Deleted)
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults).ToListAsync();


            var response = new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount,
                }
            };
            return response;
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _context.Products
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                                ||
                                p.Description.ToLower().Contains(searchText.ToLower()) && p.Visible && !p.Deleted)
                                .Include(p => p.Variants.Where(p => p.Visible && !p.Deleted)).ToListAsync();
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);
            List<string> result = new List<string>();
            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description.Split()
                        .Select(x => x.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                            && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }

                }

            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<List<Product>>> FeaturedProduct()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.Where(x => x.Featured && x.Visible && !x.Deleted).Include(a => a.Variants.Where(p => p.Visible && !p.Deleted)).ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsForAdmin()
        {
            var result = await _context.Products
                .Where(p => !p.Deleted)
                .Include(p => p.Variants
                .Where(v => !v.Deleted))
                .ThenInclude(p => p.ProductType).ToListAsync();

            return new ServiceResponse<List<Product>>
            {
                Data = result,
                Success = true,
            };
        }

        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            foreach (var variant in product.Variants)
            {
                variant.ProductType = null;
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);
            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product is not found"
                };
            }
            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;
            foreach (var variant in product.Variants)
            {
                var dbVariant = await _context.ProductVariants.SingleOrDefaultAsync(x => x.ProductId == variant.ProductId

                 && x.ProductTypeId == variant.ProductTypeId
                );
                if (dbVariant == null)
                {
                    variant.ProductType = null;
                    _context.ProductVariants.Add(variant);
                }
                else
                {
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice= variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }


            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data=product };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var dbProduct = await _context.Products.FindAsync(productId);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Product is not found"
                };
            }
            dbProduct.Deleted = true;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Success = true };
        }
    }
}
