using Apphia_Website_API.Repository.Model.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;

namespace Apphia_Website_API.Repository.Interface.Transaction {
    public interface IProductService {
        Task<int> Create(ProductCreateViewModel model, int userId, IFormFile? image);
        Task<List<ProductReadViewModel>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<List<ProductWebsiteViewModel>> ReadWebsite(string? category);
        Task<Product?> ReadById(int id);
        Task Update(ProductUpdateViewModel model, int id, int userId, IFormFile? image);
        Task Delete(int id, int userId);
        Task Restore(int id, int userId);
    }
    public interface IProductCategoryService {
        Task<int> Create(ProductCategoryCreateViewModel model, int userId);
        Task<List<ProductCategory>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<ProductCategory?> ReadById(int id);
        Task Update(ProductCategoryUpdateViewModel model, int id, int userId);
        Task Delete(int id, int userId);
        Task Restore(int id, int userId);
        Task<object> GetAll();
    }
}
