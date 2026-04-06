using Apphia_Website_API.Repository.Model.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;

namespace Apphia_Website_API.Repository.Interface.Transaction {
    public interface IGalleryAlbumService {
        Task<int> Create(GalleryAlbumCreateViewModel model, int userId, IFormFile? thumbnail);
        Task<List<GalleryAlbum>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<GalleryAlbum?> ReadById(int id);
        Task Update(GalleryAlbumUpdateViewModel model, int id, int userId, IFormFile? thumbnail);
        Task Delete(int id, int userId);
        Task Restore(int id, int userId);
        Task<object> GetAll();
    }
    public interface IGalleryPhotoService {
        Task<int> Create(GalleryPhotoCreateViewModel model, int userId, IFormFile? image);
        Task<List<GalleryPhotoReadViewModel>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<GalleryPhoto?> ReadById(int id);
        Task Update(GalleryPhotoUpdateViewModel model, int id, int userId, IFormFile? image);
        Task Delete(int id, int userId);
        Task Restore(int id, int userId);
    }
}
