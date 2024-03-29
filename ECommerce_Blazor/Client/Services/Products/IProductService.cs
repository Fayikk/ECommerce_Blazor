﻿using ECommerce_Blazor.Shared;

namespace ECommerce_Blazor.Client.Services.Products
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        List<Product> AdminProducts { get; set; }
        string Message { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int id);
        Task SearchProducts(string searchText, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchText);
        //For pagination
        int currentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        Task GetAdminProducts();
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Product product);

    }
}
