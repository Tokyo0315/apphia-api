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
    public class WorkflowController : BaseController {
        private readonly IWorkflowService _workflowService;
        private readonly IWorkflowApproverService _workflowApproverService;
        private readonly IAuditGenericService<WorkflowAudit> _auditService;

        public WorkflowController(
            IWorkflowService workflowService,
            IWorkflowApproverService workflowApproverService,
            IAuditGenericService<WorkflowAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _workflowService = workflowService;
            _workflowApproverService = workflowApproverService;
            _auditService = auditService;
        }

        [Authorize]
        [Control(AccessType.Create, Policies.Workflow)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromBody] WorkflowCreateViewModel model, int userId) {
            try {
                var result = await _workflowService.Create(model, userId);
                await _auditService.CreateLog(new WorkflowAudit {
                    Action = "Create",
                    Details = "Created workflow: " + model.WorkflowName
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Workflow created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Workflow)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _workflowService.Read(isActive, pageNumber, pageSize, filter, sort);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Workflow)]
        [HttpGet("ReadIndividual/{id}")]
        public async Task<IActionResult> ReadIndividual(int id) {
            try {
                var result = await _workflowService.ReadSF(id);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "Workflow not found", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.Workflow)]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromBody] WorkflowUpdateViewModel model, int id, int userId) {
            try {
                var result = await _workflowService.Update(model, id, userId);
                await _auditService.CreateLog(new WorkflowAudit {
                    Action = "Update",
                    Details = "Updated workflow ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Workflow updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.Workflow)]
        [HttpPost("Delete/{id}/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId) {
            try {
                var result = await _workflowService.Delete(id, userId);
                await _auditService.CreateLog(new WorkflowAudit {
                    Action = "Delete",
                    Details = "Deleted workflow ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Workflow deleted successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.Workflow)]
        [HttpPost("Restore/{id}/{userId}")]
        public async Task<IActionResult> Restore(int id, int userId) {
            try {
                var result = await _workflowService.Restore(id, userId);
                await _auditService.CreateLog(new WorkflowAudit {
                    Action = "Restore",
                    Details = "Restored workflow ID: " + id
                }, HttpContext.User);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Workflow restored successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.Restore", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.Workflow)]
        [HttpPost("WorkflowApprover/{workflowId}/{approvalType}/{userId}")]
        public async Task<IActionResult> WorkflowApprover(int workflowId, string approvalType, [FromBody] List<WorkflowApproverCreateViewModel> model, int userId) {
            try {
                var result = await _workflowApproverService.WorkflowApprover(workflowId, approvalType, model, userId);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Workflow approvers updated successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.WorkflowApprover", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.Workflow)]
        [HttpGet("ReadWorkflowApprover/{workflowId}")]
        public async Task<IActionResult> ReadWorkflowApprover(int workflowId) {
            try {
                var result = await _workflowApproverService.ReadSF(workflowId);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "WorkflowController.ReadWorkflowApprover", 500, "Internal Server Error");
            }
        }
    }
}
