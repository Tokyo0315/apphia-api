using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface.Transaction;
using Apphia_Website_API.Repository.Model.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;

namespace Apphia_Website_API.Repository.Service.Transaction {
    public class GalleryAlbumService : IGalleryAlbumService {
        private readonly DatabaseContext _context;
        public GalleryAlbumService(DatabaseContext context) { _context = context; }

        public async Task<int> Create(GalleryAlbumCreateViewModel model, int userId, IFormFile? thumbnail) {
            var entity = new GalleryAlbum { Name = model.Name, Description = model.Description, SortOrder = model.SortOrder, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            if (thumbnail != null) {
                var path = Path.Combine("wwwroot", "assets", "contents", "images", "gallery");
                Directory.CreateDirectory(path);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(thumbnail.FileName)}";
                using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await thumbnail.CopyToAsync(stream);
                entity.Thumbnail = fileName;
            }
            _context.GalleryAlbums.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<GalleryAlbum>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.GalleryAlbums.Where(a => a.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(a => a.Name.Contains(filter));
            return await query.OrderBy(a => a.SortOrder).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<GalleryAlbum?> ReadById(int id) => await _context.GalleryAlbums.Include(a => a.Photos).FirstOrDefaultAsync(a => a.Id == id);

        public async Task Update(GalleryAlbumUpdateViewModel model, int id, int userId, IFormFile? thumbnail) {
            var entity = await _context.GalleryAlbums.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.Name = model.Name; entity.Description = model.Description; entity.SortOrder = model.SortOrder;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            if (thumbnail != null) {
                var path = Path.Combine("wwwroot", "assets", "contents", "images", "gallery");
                Directory.CreateDirectory(path);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(thumbnail.FileName)}";
                using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await thumbnail.CopyToAsync(stream);
                entity.Thumbnail = fileName;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id, int userId) {
            var entity = await _context.GalleryAlbums.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task Restore(int id, int userId) {
            var entity = await _context.GalleryAlbums.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetAll() => await _context.GalleryAlbums.Where(x => x.IsActive == true).Select(x => new { Key = x.Id, Label = x.Name }).ToListAsync();
    }

    public class GalleryPhotoService : IGalleryPhotoService {
        private readonly DatabaseContext _context;
        public GalleryPhotoService(DatabaseContext context) { _context = context; }

        public async Task<int> Create(GalleryPhotoCreateViewModel model, int userId, IFormFile? image) {
            var entity = new GalleryPhoto { Caption = model.Caption, SortOrder = model.SortOrder, GalleryAlbumId = model.GalleryAlbumId, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            if (image != null) {
                var path = Path.Combine("wwwroot", "assets", "contents", "images", "gallery");
                Directory.CreateDirectory(path);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await image.CopyToAsync(stream);
                entity.Image = fileName;
            }
            _context.GalleryPhotos.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<GalleryPhotoReadViewModel>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.GalleryPhotos.Include(p => p.GalleryAlbum).Where(p => p.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(p => p.Caption != null && p.Caption.Contains(filter));

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(p => p.SortOrder).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();

            return items.Select(p => new GalleryPhotoReadViewModel {
                Id = p.Id,
                Caption = p.Caption,
                Album = p.GalleryAlbum?.Name ?? "",
                SortOrder = p.SortOrder,
                IsActive = p.IsActive,
                TotalCount = totalCount,
                DeletedBy = p.DeletedByUserId != null ? _context.UserAccounts.Include(x => x.Employee).Where(x => x.Id == p.DeletedByUserId).Select(x => x.Employee!.GivenName + " " + x.Employee.LastName).FirstOrDefault() : null,
                DeletedDate = p.DeletedDate?.ToString("MMMM d, yyyy"),
            }).ToList();
        }

        public async Task<GalleryPhoto?> ReadById(int id) => await _context.GalleryPhotos.FindAsync(id);

        public async Task Update(GalleryPhotoUpdateViewModel model, int id, int userId, IFormFile? image) {
            var entity = await _context.GalleryPhotos.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.Caption = model.Caption; entity.SortOrder = model.SortOrder; entity.GalleryAlbumId = model.GalleryAlbumId;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            if (image != null) {
                var path = Path.Combine("wwwroot", "assets", "contents", "images", "gallery");
                Directory.CreateDirectory(path);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await image.CopyToAsync(stream);
                entity.Image = fileName;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id, int userId) {
            var entity = await _context.GalleryPhotos.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task Restore(int id, int userId) {
            var entity = await _context.GalleryPhotos.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
