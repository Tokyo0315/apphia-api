using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SystemSetup;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Configuration.Attribute_Extender;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Controllers.SystemSetup {
    [Route("api/[controller]")]
    [ApiController]
    public class EmailRecipientController : BaseController {
        private readonly IEmailRecipientService _emailRecipientService;
        private readonly IAuditGenericService<EmailRecipientAudit> _auditService;

        public EmailRecipientController(
            IEmailRecipientService emailRecipientService,
            IAuditGenericService<EmailRecipientAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _emailRecipientService = emailRecipientService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.EmailRecipient)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromBody] EmailRecipientViewModel model, int userId) {
            try {
                var result = await _emailRecipientService.InsertEmail(model, userId);
                await _auditService.CreateLog(new EmailRecipientAudit {
                    Action = "Create",
                    Details = "Created email recipient: " + model.email
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Email Recipient created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmailRecipientController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.EmailRecipient)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _emailRecipientService.ReadRecipients(isActive, pageNumber, pageSize, filter, sort);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmailRecipientController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.EmailRecipient)]
        [HttpGet("ReadIndividual/{emailId}")]
        public async Task<IActionResult> ReadIndividual(int emailId) {
            try {
                var result = await _emailRecipientService.ReadRecipient(emailId);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Email Recipient not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmailRecipientController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.EmailRecipient)]
        [HttpGet("ReadBySegment/{segment}")]
        public async Task<IActionResult> ReadBySegment(string segment) {
            try {
                var result = await _emailRecipientService.ReadRecipientBaseOnSegment(segment);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmailRecipientController.ReadBySegment", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.EmailRecipient)]
        [HttpPost("Update/{emailId}/{userId}")]
        public async Task<IActionResult> Update([FromBody] EmailRecipientViewModel model, int emailId, int userId) {
            try {
                var result = await _emailRecipientService.UpdateEmail(model, emailId, userId);
                await _auditService.CreateLog(new EmailRecipientAudit {
                    Action = "Update",
                    Details = "Updated email recipient ID: " + emailId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Email Recipient updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmailRecipientController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.EmailRecipient)]
        [HttpPost("Delete/{emailId}/{userId}")]
        public async Task<IActionResult> Delete(int emailId, int userId) {
            try {
                var result = await _emailRecipientService.DeleteEmail(emailId, userId);
                await _auditService.CreateLog(new EmailRecipientAudit {
                    Action = "Delete",
                    Details = "Deleted email recipient ID: " + emailId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Email Recipient deleted successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmailRecipientController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.EmailRecipient)]
        [HttpPost("Restore/{emailId}/{userId}")]
        public async Task<IActionResult> Restore(int emailId, int userId) {
            try {
                var result = await _emailRecipientService.RestoreEmail(emailId, userId);
                await _auditService.CreateLog(new EmailRecipientAudit {
                    Action = "Restore",
                    Details = "Restored email recipient ID: " + emailId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Email Recipient restored successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "EmailRecipientController.Restore", 500, "Internal Server Error");
            }
        }
    }
}
