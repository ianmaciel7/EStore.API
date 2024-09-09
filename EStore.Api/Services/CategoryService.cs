using EStore.Api.Exceptions;
using EStore.Api.InputModel;
using EStore.Api.Repository;
using EStore.Api.ViewModel;
using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.InputModel;
using EStore.API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public async Task<CategoryOutputModel> GetCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryAsync(categoryId);

            if (!IsThereThisCategory(category))
                throw new CategoryNotFoundException(categoryId);

            return new CategoryOutputModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                SubCategories = category.SubCategories                
            };
        }
       
        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryAsync(categoryId);

            if (!IsThereThisCategory(category))
                throw new CategoryNotFoundException(categoryId);

            if (!category.SubCategories.Any())
                throw new CategoryContainsSubCategoriesException(categoryId);

            _categoryRepository.DeleteCategory(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<CategoryOutputModel> AddCategoryAsync(CategoryInputModel model)
        {
            
            var category = new Category()
            {               
                Name = model.Name,               
            };
                      
            if (await IsThereThisCategoryAsync(model.Name))
                throw new CategoryNameNotUniqueException(model.Name);
         
            var addedCategory = _categoryRepository.AddCategoryAsync(category).Result;
            await _categoryRepository.SaveChangesAsync();
            return new CategoryOutputModel
            {
                CategoryId = addedCategory.CategoryId,
                Name = addedCategory.Name,
                SubCategories = addedCategory.SubCategories               
            };

        }

        public async Task<CategoryOutputModel> UpdateCategoryAsync(int categoryId, CategoryInputModel model)
        {
            var categoryOld = await _categoryRepository.GetCategoryAsync(categoryId);
            
            if (!IsThereThisCategory(categoryOld))
                throw new CategoryNotFoundException(categoryId);

            if (await IsThereThisCategoryAsync(model.Name))
                throw new CategoryNameNotUniqueException(model.Name);

            var category = new Category()
            {
                CategoryId = categoryOld.CategoryId,
                SubCategories = categoryOld.SubCategories,
                Name = model.Name
            };

            _categoryRepository.UpdateCategory(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryOutputModel
            {
                CategoryId = category.CategoryId,                
                Name = category.Name,
                SubCategories = category.SubCategories
            };
        }

        public async Task<IEnumerable<CategoryOutputModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryOutputModel
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                SubCategories = c.SubCategories
            });
        }

        public async Task<IEnumerable<SubCategoryOuputModel>> GetAllSubCategoriesAsync(string categoryName)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);
           
            var subCategories = await _categoryRepository.GetAllSubCategoriesAsync(categoryName);
            return subCategories.Select(s => new SubCategoryOuputModel
            {
                SubCategoryId = s.SubCategoryId,
                Name = s.Name,
                Products = s.Products
            });
        }

        public async Task<SubCategoryOuputModel> GetSubCategoryAsync(string categoryName, int subCategoryId)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            var subCategory = await _categoryRepository.GetSubCategoryAsync(categoryName, subCategoryId);

            if (!IsThereThisSubCategory(subCategory))
                throw new SubCategoryNotFoundException(categoryName,subCategoryId);

            return new SubCategoryOuputModel
            {
                SubCategoryId = subCategory.SubCategoryId,
                Name = subCategory.Name,
                Products = subCategory.Products
            };
        }

        public async Task<SubCategoryOuputModel> AddSubCategoryAsync(string categoryName, CategoryInputModel model)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (await IsThereThisSubCategoryAsync(model.Name))
                throw new SubCategoryNameNotUniqueException(model.Name);

            var subCategory = new SubCategory()
            {               
                Name = model.Name
            };

            var addedSubCategory = await _categoryRepository.AddSubCategoryAsync(categoryName,subCategory);
            await _categoryRepository.SaveChangesAsync();

            return new SubCategoryOuputModel
            {
                SubCategoryId = addedSubCategory.SubCategoryId,
                Name = addedSubCategory.Name,
                Products = addedSubCategory.Products
            };
        }
       
        public async Task<SubCategoryOuputModel> UpdateSubCategoryAsync(string categoryName, int subCategoryId, SubCategoryInputModel model)
        {
            var subCategoryOld = await _categoryRepository.GetSubCategoryAsync(categoryName,subCategoryId);

            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!IsThereThisSubCategory(subCategoryOld))
                throw new SubCategoryNotFoundException(categoryName,subCategoryId);

            if (await IsThereThisSubCategoryAsync(model.Name))
                throw new SubCategoryNameNotUniqueException(model.Name);

            var subCategory = new SubCategory()
            {
                SubCategoryId = subCategoryOld.SubCategoryId,
                Category = subCategoryOld.Category,
                Products = subCategoryOld.Products,
                Name = model.Name
            };

            _categoryRepository.UpdateSubCategory(subCategory);
            await _categoryRepository.SaveChangesAsync();

            return new SubCategoryOuputModel
            {
                SubCategoryId = subCategory.SubCategoryId,
                Name = subCategory.Name,
                Products = subCategory.Products
                
            };

        }

        public async Task<IEnumerable<ProductOutputModel>> GetAllProductsAsync(string categoryName, string subCategoryName, int page, int quantity)
        {

            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var products = await _categoryRepository.GetAllProductsAsync(categoryName, subCategoryName, page, quantity);

            return products.Select(p => new ProductOutputModel
            {
                ProductId = p.ProductId,
                Price = p.Price,
                Name = p.Name              
            });
        }

        public async Task<ProductOutputModel> GetProductAsync(string categoryName, string subCategoryName, int productId)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var product = await _categoryRepository.GetProductAsync(categoryName, subCategoryName, productId);

            if (!IsThereThisProduct(product))
                throw new ProductNotFoundException(categoryName,productId);

            return new ProductOutputModel
            {
                ProductId = product.ProductId,
                Price = product.Price,
                Name = product.Name
            };
        }

        public async Task<ProductOutputModel> AddProductAsync(string categoryName, string subCategoryName, ProductInputModel model)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            if (await IsThereThisProductAsync(model.Name))
                throw new ProductNameNotUniqueException(model.Name);

            var product = new Product()
            {
                Price = model.Price,
                Name = model.Name,  
            };

            var addedProduct = await _categoryRepository.AddProductAsync(subCategoryName,product);
            await _categoryRepository.SaveChangesAsync();
            return new ProductOutputModel
            {
                ProductId = addedProduct.ProductId,
                Price = addedProduct.Price,
                Name = addedProduct.Name
            };

        }

        public async Task<ProductOutputModel> UpdateProductAsync(string categoryName, string subCategoryName, int productId, ProductInputModel model)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var product = await _categoryRepository.GetProductAsync(categoryName, subCategoryName, productId);

            if (!IsThereThisProduct(product))            
                throw new ProductNotFoundException(subCategoryName, productId);            
                
            if (await IsThereThisProductAsync(model.Name))
                throw new ProductNameNotUniqueException(model.Name);

            var newProduct = new Product()
            {
                ProductId = productId,
                Price = model.Price,
                Name = model.Name,
            };

            _categoryRepository.UpdateProduct(newProduct);
            await _categoryRepository.SaveChangesAsync();

            return new ProductOutputModel
            {
                ProductId = newProduct.ProductId,
                Price = newProduct.Price,
                Name = newProduct.Name
            };

        }

        public async Task DeleteProductAsync(string categoryName, string subCategoryName, int productId)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var product = await _categoryRepository.GetProductAsync(categoryName, subCategoryName, productId);

            if (!IsThereThisProduct(product))
                throw new ProductNotFoundException(subCategoryName, productId);

            _categoryRepository.DeleteProduct(product);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteSubCategoryAsync(string categoryName, int subCategoryId)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            var subCategory = await _categoryRepository.GetSubCategoryAsync(categoryName, subCategoryId);

            if (!IsThereThisSubCategory(subCategory))
                throw new SubCategoryNotFoundException(categoryName, subCategoryId);

            if (subCategory.Products.Any())
                throw new SubCategoriesContainsProductsException(subCategoryId);

            _categoryRepository.DeleteSubCategory(subCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        private async Task<bool> IsThereThisCategoryAsync(string categoryName)
        {
            var existing = await _categoryRepository.GetCategoryAsync(categoryName);
            if (existing != null) return true;
            return false;
        }

        private async Task<bool> IsThereThisSubCategoryAsync(string categoryName, string subCategoryName)
        {
            var existing = await _categoryRepository.GetSubCategoryAsync(categoryName, subCategoryName);
            if (existing != null) return true;
            return false;
        }

        private async Task<bool> IsThereThisSubCategoryAsync(string subCategoryName)
        {
            var existing = await _categoryRepository.GetSubCategoryAsync(subCategoryName);
            if (existing != null) return true;
            return false;
        }

        private async Task<bool> IsThereThisProductAsync(string productName)
        {
            var existing = await _categoryRepository.GetProductAsync(productName);
            if (existing != null) return true;
            return false;
        }

        private bool IsThereThisProduct(Product product)
        {
            var existing = product;
            if (existing != null) return true;
            return false;
        }

        private bool IsThereThisSubCategory(SubCategory subCategory)
        {
            var existing = subCategory;
            if (existing != null) return true;
            return false;
        }

        private bool IsThereThisCategory(Category category)
        {
            var existing = category;
            if (existing != null) return true;
            return false;
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }
        
    }
}
