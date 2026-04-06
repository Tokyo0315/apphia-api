using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Controllers.Core {
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(
            IEmployeeService employeeService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _employeeService = employeeService;
        }

        [Authorize]
        [HttpGet("ReadEmployee/{employeeId}")]
        public async Task<IActionResult> ReadEmployee(int employeeId) {
            try {
                var result = await _employeeService.ReadEmployee(employeeId);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Employee not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmployeeController.ReadEmployee", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost("UpdateEmployee/{employeeId}/{userId}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdateViewModel model, int employeeId, int userId) {
            try {
                var result = await _employeeService.EditEmployee(model, employeeId, userId);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Employee updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmployeeController.UpdateEmployee", 500, "Internal Server Error");
            }
        }
    }
}
