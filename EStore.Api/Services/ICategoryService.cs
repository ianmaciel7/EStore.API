using EStore.Api.InputModel;
using EStore.Api.ViewModel;
using EStore.Api.InputModel;
using EStore.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Services
{
    public interface ICategoryService : IDisposable
    {
        Task<CategoryOutputModel> UpdateCategoryAsync(int categoryId, CategoryInputModel model);
        Task<CategoryOutputModel> GetCategoryAsync(int CategoryId);
        Task<CategoryOutputModel> AddCategoryAsync(CategoryInputModel model);
        Task<IEnumerable<CategoryOutputModel>> GetAllCategoriesAsync();
        Task<IEnumerable<SubCategoryOuputModel>> GetAllSubCategoriesAsync(string categoryName);       
        Task<SubCategoryOuputModel> GetSubCategoryAsync(string categoryName, int subCategoryId);
        Task<SubCategoryOuputModel> AddSubCategoryAsync(string categoryName, CategoryInputModel model);
        Task<IEnumerable<ProductOutputModel>> GetAllProductsAsync(string categoryName, string subCategoryName, int page,int quantity);
        Task<ProductOutputModel> AddProductAsync(string categoryName, string subCategoryName, ProductInputModel model);
        Task<ProductOutputModel> GetProductAsync(string categoryName, string subCategoryName, int productId);       
        Task<ProductOutputModel> UpdateProductAsync(string categoryName, string subCategoryName, int productId, ProductInputModel model);
        Task DeleteProductAsync(string categoryName, string subCategoryName, int productId);
        Task DeleteCategoryAsync(int categoryId);
        Task<SubCategoryOuputModel> UpdateSubCategoryAsync(string categoryName, int categoryId, SubCategoryInputModel model);
        Task DeleteSubCategoryAsync(string categoryName, int subCategoryId);
    }
}
