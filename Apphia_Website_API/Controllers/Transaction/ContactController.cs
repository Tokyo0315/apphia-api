using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Configuration.Attribute_Extender;
using Apphia_Website_API.Repository.Configuration.Enum;
using Apphia_Website_API.Repository.Interface.SystemSetup;

namespace Apphia_Website_API.Controllers.Transaction {
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : BaseController {
        private readonly IContactService _contactService;
        private readonly IAuditGenericService<ContactAudit> _auditService;
        private readonly IApiService _apiService;
        private readonly IEmailRecipientService _emailRecipientService;

        public ContactController(
            IContactService contactService,
            IAuditGenericService<ContactAudit> auditService,
            IApiService apiService,
            IEmailRecipientService emailRecipientService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _contactService = contactService;
            _auditService = auditService;
            _apiService = apiService;
            _emailRecipientService = emailRecipientService;
        }

        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs([FromBody] ContactUsViewModel model) {
            try {
                var result = await _contactService.Create(model);

                // Build template parameters from form data
                var jsonData = model.GetType().GetProperties().Select(p => new {
                    Name = p.Name,
                    Value = p.GetValue(model)?.ToString() ?? string.Empty,
                }).ToList();

                // Send email notification to all admin recipients with segment "contact"
                var recipients = await _emailRecipientService.ReadRecipientBaseOnSegment("contact");
                foreach (var r in recipients) {
                    await _apiService.Send(r.email, "contact-us", jsonData);
                }

                // Send confirmation email to the user
                if (!string.IsNullOrEmpty(model.email)) {
                    await _apiService.Send(model.email, "contact-us-response", jsonData);
                }

                return StatusCode(200, _requestStatusHelper.response(200, true, "Message sent successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ContactController.ContactUs", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Contact)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _contactService.ReadContacts(pageNumber, pageSize, isActive, filter, sort);
                int? totalCount = result.Count > 0 ? result[0].TotalCount : 0;
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, totalCount));
            } catch (Exception ex) {
                return await HandleException(ex, "ContactController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Contact)]
        [HttpGet("ReadIndividual/{contactId}")]
        public async Task<IActionResult> ReadIndividual(int contactId) {
            try {
                var result = await _contactService.ReadContact(contactId);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Contact not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ContactController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.Contact)]
        [HttpPost("Delete/{contactId}/{userId}")]
        public async Task<IActionResult> Delete(int contactId, int userId) {
            try {
                var result = await _contactService.DeleteContact(contactId, userId);
                await _auditService.CreateLog(new ContactAudit {
                    Action = "Delete",
                    Details = "Deleted contact ID: " + contactId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Contact deleted successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ContactController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.Contact)]
        [HttpPost("Restore/{contactId}/{userId}")]
        public async Task<IActionResult> Restore(int contactId, int userId) {
            try {
                var result = await _contactService.RestoreContact(contactId, userId);
                await _auditService.CreateLog(new ContactAudit {
                    Action = "Restore",
                    Details = "Restored contact ID: " + contactId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Contact restored successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ContactController.Restore", 500, "Internal Server Error");
            }
        }
    }
}
