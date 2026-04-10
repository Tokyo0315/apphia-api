using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Configuration;
using Apphia_Website_API.Repository.Configuration.Helper;

namespace Apphia_Website_API.Controllers {
    public abstract class BaseController : ControllerBase {
        protected readonly IConfiguration _configuration;
        protected readonly IAuditLogService _auditLogService;
        protected readonly IRequestStatusHelper _requestStatusHelper;

        protected BaseController(IConfiguration configuration, IAuditLogService auditLogService, IRequestStatusHelper requestStatusHelper) {
            _configuration = configuration;
            _auditLogService = auditLogService;
            _requestStatusHelper = requestStatusHelper;
        }

        protected async Task<IActionResult> HandleException(Exception ex, string method, int statusCode, string? message) {
            var destinationPath = _configuration.GetSection("DestinationPath").Get<DestinationPath>();
            var path = destinationPath?.ErrorPath ?? "";
            await _auditLogService.LogStatusToFileAsync(
                $"Message: {ex.Message} Source: {ex.Source} InnerException: {ex.InnerException}",
                path, method, "Error");
            return StatusCode(statusCode, _requestStatusHelper.response(statusCode, false, message, ex.Message, null));
        }
    }
}
