using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Configuration.Attribute_Extender;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Controllers.Transaction {
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryAlbumController : BaseController {
        private readonly IGalleryAlbumService _galleryAlbumService;
        private readonly IAuditGenericService<GalleryAlbumAudit> _auditService;

        public GalleryAlbumController(
            IGalleryAlbumService galleryAlbumService,
            IAuditGenericService<GalleryAlbumAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _galleryAlbumService = galleryAlbumService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.GalleryAlbum)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromForm] GalleryAlbumCreateViewModel model, IFormFile? thumbnail, int userId) {
            try {
                var result = await _galleryAlbumService.Create(model, userId, thumbnail);
                await _auditService.CreateLog(new GalleryAlbumAudit {
                    Action = "Create",
                    Details = "Created gallery album: " + model.Name
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Album created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryAlbumController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.GalleryAlbum)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _galleryAlbumService.Read(isActive, pageNumber, pageSize, filter, sort);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryAlbumController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.GalleryAlbum)]
        [HttpGet("ReadIndividual/{id}")]
        public async Task<IActionResult> ReadIndividual(int id) {
            try {
                var result = await _galleryAlbumService.ReadById(id);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Gallery Album not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryAlbumController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.GalleryAlbum)]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromForm] GalleryAlbumUpdateViewModel model, IFormFile? thumbnail, int id, int userId) {
            try {
                await _galleryAlbumService.Update(model, id, userId, thumbnail);
                await _auditService.CreateLog(new GalleryAlbumAudit {
                    Action = "Update",
                    Details = "Updated gallery album ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Album updated successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryAlbumController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.GalleryAlbum)]
        [HttpPost("Delete/{id}/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId) {
            try {
                await _galleryAlbumService.Delete(id, userId);
                await _auditService.CreateLog(new GalleryAlbumAudit {
                    Action = "Delete",
                    Details = "Deleted gallery album ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Album deleted successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryAlbumController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.GalleryAlbum)]
        [HttpPost("Restore/{id}/{userId}")]
        public async Task<IActionResult> Restore(int id, int userId) {
            try {
                await _galleryAlbumService.Restore(id, userId);
                await _auditService.CreateLog(new GalleryAlbumAudit {
                    Action = "Restore",
                    Details = "Restored gallery album ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Album restored successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryAlbumController.Restore", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
            try {
                var result = await _galleryAlbumService.GetAll();
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryAlbumController.GetAll", 500, "Internal Server Error");
            }
        }
    }
}
