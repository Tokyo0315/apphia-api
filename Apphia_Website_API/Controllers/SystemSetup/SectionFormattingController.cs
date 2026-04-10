using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SectionFormatting;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Configuration.Attribute_Extender;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Controllers.SystemSetup {
    [Route("api/[controller]")]
    [ApiController]
    public class SectionFormattingController : BaseController {
        private readonly ISectionFormattingService _sectionFormattingService;
        private readonly IAuditGenericService<SectionFormattingAudit> _auditService;

        public SectionFormattingController(
            ISectionFormattingService sectionFormattingService,
            IAuditGenericService<SectionFormattingAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _sectionFormattingService = sectionFormattingService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.SectionFormatting)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromBody] SectionFormattingCreateViewModel model, int userId) {
            try {
                var result = await _sectionFormattingService.Create(model, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Create",
                    Details = "Created section formatting: " + model.Name
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Section Formatting created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.SectionFormatting)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _sectionFormattingService.Read(isActive, pageNumber, pageSize, filter, sort);
                int? totalCount = result.Count > 0 ? result[0].TotalCount : 0;
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, totalCount));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.SectionFormatting)]
        [HttpGet("ReadIndividual/{id}/{isActive?}")]
        public async Task<IActionResult> ReadIndividual(int id, bool isActive = true) {
            try {
                var result = await _sectionFormattingService.ReadSF(id, isActive);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Section Formatting not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.SectionFormatting)]
        [HttpGet("ReadGrapesJS/{id}")]
        public async Task<IActionResult> ReadGrapesJS(int id) {
            try {
                var result = await _sectionFormattingService.ReadGrapesJS(id);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.ReadGrapesJS", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("ReadTabs")]
        public async Task<IActionResult> ReadTabs() {
            try {
                var result = await _sectionFormattingService.ReadTabs();
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.ReadTabs", 500, "Internal Server Error");
            }
        }

        [HttpGet("Read/Website/{page}")]
        public async Task<IActionResult> ReadWebsite(string page) {
            try {
                var result = await _sectionFormattingService.ReadWebsite(page);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.ReadWebsite", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.SectionFormatting)]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromBody] SectionFormattingUpdateViewModel model, int id, int userId) {
            try {
                var result = await _sectionFormattingService.Update(model, id, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Update",
                    Details = "Updated section formatting ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Section Formatting updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.SectionFormatting)]
        [HttpPost("UpdateGrapesJS/{id}/{userId}")]
        public async Task<IActionResult> UpdateGrapesJS([FromBody] SectionFormattingGrapesJSViewModel model, int id, int userId) {
            try {
                var result = await _sectionFormattingService.UpdateGrapesJS(model, id, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Update GrapesJS",
                    Details = "Updated GrapesJS content for section formatting ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "GrapesJS content updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.UpdateGrapesJS", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.SectionFormatting)]
        [HttpPost("Delete/{id}/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId) {
            try {
                var result = await _sectionFormattingService.Delete(id, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Delete",
                    Details = "Deleted section formatting ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Section Formatting deleted successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.SectionFormatting)]
        [HttpPost("Restore/{id}/{userId}")]
        public async Task<IActionResult> Restore(int id, int userId) {
            try {
                var result = await _sectionFormattingService.Restore(id, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Restore",
                    Details = "Restored section formatting ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Section Formatting restored successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Restore", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost("Route/{id}/{workflowId}/{userId}")]
        public async Task<IActionResult> Route(int id, int workflowId, int userId) {
            try {
                var result = await _sectionFormattingService.Route(id, workflowId, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Route",
                    Details = "Routed section formatting ID: " + id + " to workflow ID: " + workflowId
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Routed successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Route", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost("Remind/{id}/{userId}/{remind?}")]
        public async Task<IActionResult> Remind(int id, int userId, bool remind = true) {
            try {
                var result = await _sectionFormattingService.Remind(id, userId, remind);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Reminder sent successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Remind", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost("Recall/{id}/{userId}")]
        public async Task<IActionResult> Recall(int id, int userId) {
            try {
                var result = await _sectionFormattingService.Recall(id, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Recall",
                    Details = "Recalled section formatting ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Recalled successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Recall", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost("Publish/{id}/{userId}")]
        public async Task<IActionResult> Publish(int id, int userId) {
            try {
                var result = await _sectionFormattingService.Publish(id, userId);
                await _auditService.CreateLog(new SectionFormattingAudit {
                    Action = "Publish",
                    Details = "Published section formatting ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Published successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.Publish", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("ReadApprovalStatus/{id}")]
        public async Task<IActionResult> ReadApprovalStatus(int id) {
            try {
                var result = await _sectionFormattingService.ReadApprovalStatus(id);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "SectionFormattingController.ReadApprovalStatus", 500, "Internal Server Error");
            }
        }
    }
}
