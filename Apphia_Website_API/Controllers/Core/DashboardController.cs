using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Controllers.Core {
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController {
        private readonly IDashboardService _dashboardService;

        public DashboardController(
            IDashboardService dashboardService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _dashboardService = dashboardService;
        }

        [Authorize]
        [HttpGet("Read/LatestGeneratedDate")]
        public async Task<IActionResult> ReadLatestGeneratedDate() {
            try {
                var result = await _dashboardService.ReadLatestGeneratedDate();
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "DashboardController.ReadLatestGeneratedDate", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("Read/Pages/{generatedDate}/{domain}")]
        public async Task<IActionResult> ReadPages(DateTime generatedDate, string domain) {
            try {
                var result = await _dashboardService.ReadPages(generatedDate, domain);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "DashboardController.ReadPages", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("Read/Devices/{generatedDate}/{domain}")]
        public async Task<IActionResult> ReadDevices(DateTime generatedDate, string domain) {
            try {
                var result = await _dashboardService.ReadDevices(generatedDate, domain);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "DashboardController.ReadDevices", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("Read/Countries/{generatedDate}/{domain}")]
        public async Task<IActionResult> ReadCountries(DateTime generatedDate, string domain) {
            try {
                var result = await _dashboardService.ReadCountries(generatedDate, domain);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "DashboardController.ReadCountries", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("Read/Engagements/{generatedDate}/{domain}")]
        public async Task<IActionResult> ReadEngagements(DateTime generatedDate, string domain) {
            try {
                var result = await _dashboardService.ReadEngagements(generatedDate, domain);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "DashboardController.ReadEngagements", 500, "Internal Server Error");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DashboardCreateViewModel model) {
            try {
                await _dashboardService.Create(model);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Dashboard data created successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "DashboardController.Create", 500, "Internal Server Error");
            }
        }
    }
}
