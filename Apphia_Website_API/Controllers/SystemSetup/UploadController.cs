using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;

namespace Apphia_Website_API.Controllers.SystemSetup {
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController {

        public UploadController(
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) { }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(IFormFile file) {
            try {
                if (file == null || file.Length == 0)
                    return StatusCode(400, _requestStatusHelper.response(400, false, "No file uploaded", null, null));

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "contents", "images", "grapesjs");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await file.CopyToAsync(stream);
                }

                var relativePath = "/assets/contents/images/grapesjs/" + uniqueFileName;
                return StatusCode(200, _requestStatusHelper.response(200, true, "File uploaded successfully", relativePath, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UploadController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("Read")]
        public IActionResult Read() {
            try {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "contents", "images", "grapesjs");
                if (!Directory.Exists(uploadsFolder))
                    return StatusCode(200, _requestStatusHelper.response(200, true, "Success", new List<string>(), null));

                var files = Directory.GetFiles(uploadsFolder)
                    .Select(f => "/assets/contents/images/grapesjs/" + Path.GetFileName(f))
                    .ToList();

                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", files, null));
            } catch (Exception ex) {
                return StatusCode(500, _requestStatusHelper.response(500, false, "Internal Server Error", ex.Message, null));
            }
        }

        [Authorize]
        [HttpPost("Delete")]
        public IActionResult Delete([FromQuery] string filePath) {
            try {
                if (string.IsNullOrEmpty(filePath))
                    return StatusCode(400, _requestStatusHelper.response(400, false, "File path is required", null, null));

                var fileName = Path.GetFileName(filePath);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "contents", "images", "grapesjs", fileName);

                if (!System.IO.File.Exists(fullPath))
                    return StatusCode(404, _requestStatusHelper.response(404, false, "File not found", null, null));

                System.IO.File.Delete(fullPath);
                return StatusCode(200, _requestStatusHelper.response(200, true, "File deleted successfully", null, null));
            } catch (Exception ex) {
                return StatusCode(500, _requestStatusHelper.response(500, false, "Internal Server Error", ex.Message, null));
            }
        }
    }
}
