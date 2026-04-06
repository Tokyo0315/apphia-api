using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface.Transaction;
using Apphia_Website_API.Repository.Model.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;

namespace Apphia_Website_API.Repository.Service.Transaction {
    public class ProductService : IProductService {
        private readonly DatabaseContext _context;
        public ProductService(DatabaseContext context) { _context = context; }

        public async Task<int> Create(ProductCreateViewModel model, int userId, IFormFile? image) {
            var entity = new Product { Name = model.Name, Description = model.Description, ProductCategoryId = model.ProductCategoryId, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            if (image != null) {
                var path = Path.Combine("wwwroot", "assets", "contents", "images", "products");
                Directory.CreateDirectory(path);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await image.CopyToAsync(stream);
                entity.Image = fileName;
            }
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<ProductReadViewModel>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.Products.Include(p => p.ProductCategory).Where(p => p.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(p => p.Name.Contains(filter));

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(p => p.CreatedDate).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();

            return items.Select(p => new ProductReadViewModel {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Category = p.ProductCategory?.Name ?? "",
                IsActive = p.IsActive,
                TotalCount = totalCount,
                DeletedBy = p.DeletedByUserId != null ? _context.UserAccounts.Include(x => x.Employee).Where(x => x.Id == p.DeletedByUserId).Select(x => x.Employee!.GivenName + " " + x.Employee.LastName).FirstOrDefault() : null,
                DeletedDate = p.DeletedDate?.ToString("MMMM d, yyyy"),
            }).ToList();
        }

        public async Task<Product?> ReadById(int id) => await _context.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);

        public async Task Update(ProductUpdateViewModel model, int id, int userId, IFormFile? image) {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.Name = model.Name; entity.Description = model.Description; entity.ProductCategoryId = model.ProductCategoryId;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            if (image != null) {
                var path = Path.Combine("wwwroot", "assets", "contents", "images", "products");
                Directory.CreateDirectory(path);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await image.CopyToAsync(stream);
                entity.Image = fileName;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id, int userId) {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task Restore(int id, int userId) {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public class ProductCategoryService : IProductCategoryService {
        private readonly DatabaseContext _context;
        public ProductCategoryService(DatabaseContext context) { _context = context; }

        public async Task<int> Create(ProductCategoryCreateViewModel model, int userId) {
            var entity = new ProductCategory { Name = model.Name, Description = model.Description, SortOrder = model.SortOrder, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.ProductCategories.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<ProductCategory>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.ProductCategories.Where(pc => pc.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(pc => pc.Name.Contains(filter));
            return await query.OrderBy(pc => pc.SortOrder).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<ProductCategory?> ReadById(int id) => await _context.ProductCategories.Include(pc => pc.Products).FirstOrDefaultAsync(pc => pc.Id == id);

        public async Task Update(ProductCategoryUpdateViewModel model, int id, int userId) {
            var entity = await _context.ProductCategories.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.Name = model.Name; entity.Description = model.Description; entity.SortOrder = model.SortOrder;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id, int userId) {
            var entity = await _context.ProductCategories.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task Restore(int id, int userId) {
            var entity = await _context.ProductCategories.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetAll() => await _context.ProductCategories.Where(x => x.IsActive == true).Select(x => new { Key = x.Id, Label = x.Name }).ToListAsync();
    }
}
