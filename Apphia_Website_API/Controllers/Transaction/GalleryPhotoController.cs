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
    public class GalleryPhotoController : BaseController {
        private readonly IGalleryPhotoService _galleryPhotoService;
        private readonly IAuditGenericService<GalleryPhotoAudit> _auditService;

        public GalleryPhotoController(
            IGalleryPhotoService galleryPhotoService,
            IAuditGenericService<GalleryPhotoAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _galleryPhotoService = galleryPhotoService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.GalleryPhoto)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromForm] GalleryPhotoCreateViewModel model, IFormFile? image, int userId) {
            try {
                var result = await _galleryPhotoService.Create(model, userId, image);
                await _auditService.CreateLog(new GalleryPhotoAudit {
                    Action = "Create",
                    Details = "Created gallery photo with caption: " + (model.Caption ?? "N/A")
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Photo created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryPhotoController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.GalleryPhoto)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _galleryPhotoService.Read(isActive, pageNumber, pageSize, filter, sort);
                int? totalCount = result.Count > 0 ? result[0].TotalCount : 0;
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, totalCount));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryPhotoController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.GalleryPhoto)]
        [HttpGet("ReadIndividual/{id}")]
        public async Task<IActionResult> ReadIndividual(int id) {
            try {
                var result = await _galleryPhotoService.ReadById(id);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Gallery Photo not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryPhotoController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.GalleryPhoto)]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromForm] GalleryPhotoUpdateViewModel model, IFormFile? image, int id, int userId) {
            try {
                await _galleryPhotoService.Update(model, id, userId, image);
                await _auditService.CreateLog(new GalleryPhotoAudit {
                    Action = "Update",
                    Details = "Updated gallery photo ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Photo updated successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryPhotoController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.GalleryPhoto)]
        [HttpPost("Delete/{id}/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId) {
            try {
                await _galleryPhotoService.Delete(id, userId);
                await _auditService.CreateLog(new GalleryPhotoAudit {
                    Action = "Delete",
                    Details = "Deleted gallery photo ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Photo deleted successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryPhotoController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.GalleryPhoto)]
        [HttpPost("Restore/{id}/{userId}")]
        public async Task<IActionResult> Restore(int id, int userId) {
            try {
                await _galleryPhotoService.Restore(id, userId);
                await _auditService.CreateLog(new GalleryPhotoAudit {
                    Action = "Restore",
                    Details = "Restored gallery photo ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Gallery Photo restored successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "GalleryPhotoController.Restore", 500, "Internal Server Error");
            }
        }
    }
}
