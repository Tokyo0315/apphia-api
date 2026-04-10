using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.ViewModel.Request.SetupSecurity;

namespace Apphia_Website_API.Controllers.SystemSetup {
    [Route("api/[controller]")]
    [ApiController]
    public class SetupSecurityController : BaseController {
        private readonly ISetupSecurityManagementService _setupSecurityService;
        private readonly IAuditGenericService<SetupSecurityAudit> _auditService;

        public SetupSecurityController(
            ISetupSecurityManagementService setupSecurityService,
            IAuditGenericService<SetupSecurityAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _setupSecurityService = setupSecurityService;
            _auditService = auditService;
        }

        [Authorize]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromBody] RequestUpdateSetupSecurityVM model, int id, int userId) {
            try {
                var securityModel = new SetupSecurityManagement {
                    FailedAttempt = model.FailedAttempt,
                    LockTimeOut = model.LockTimeOut,
                    IsDisableTimeOut = model.IsDisableTimeOut
                };
                var result = await _setupSecurityService.UpdateAsync(securityModel, id, userId);
                await _auditService.CreateLog(new SetupSecurityAudit {
                    Action = "Update",
                    Details = "Updated setup security ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Security settings updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SetupSecurityController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("Read")]
        public async Task<IActionResult> Read() {
            try {
                var result = await _setupSecurityService.ReadSecurity();
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SetupSecurityController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("ReadIndividual/{id}")]
        public async Task<IActionResult> ReadIndividual(int id) {
            try {
                var result = await _setupSecurityService.ReadOneSecurity(id);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Security setting not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SetupSecurityController.ReadIndividual", 500, "Internal Server Error");
            }
        }
    }
}
