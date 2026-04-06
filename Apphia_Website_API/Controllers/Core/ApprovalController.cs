using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Controllers.Core {
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : BaseController {
        private readonly IApprovalService _approvalService;
        private readonly IAuditGenericService<SectionFormattingAudit> _sectionFormattingAuditService;

        public ApprovalController(
            IApprovalService approvalService,
            IAuditGenericService<SectionFormattingAudit> sectionFormattingAuditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _approvalService = approvalService;
            _sectionFormattingAuditService = sectionFormattingAuditService;
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> Token(string token) {
            try {
                var result = await _approvalService.GetToken(token);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ApprovalController.Token", 500, "Internal Server Error");
            }
        }

        [HttpPost("{token}/Approve")]
        public async Task<IActionResult> Approve(string token) {
            try {
                var data = await _approvalService.GetToken(token);
                await _sectionFormattingAuditService.CreateLog(new SectionFormattingAudit {
                    Action = "Approve",
                    Details = $"{data.Title} | {data.EmailAddress}"
                }, User);

                var approval = new ApprovalProcessViewModel { IsApprove = true };
                var result = await _approvalService.ApprovalProcess(token, approval);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ApprovalController.Approve", 500, "Internal Server Error");
            }
        }

        [HttpPost("{token}/Reject")]
        public async Task<IActionResult> Reject(string token, [FromBody] string reason) {
            try {
                var data = await _approvalService.GetToken(token);
                await _sectionFormattingAuditService.CreateLog(new SectionFormattingAudit {
                    Action = "Reject",
                    Details = $"{data.Title} | {data.EmailAddress} | {reason}"
                }, User);

                var approval = new ApprovalProcessViewModel { IsApprove = false, Reason = reason };
                var result = await _approvalService.ApprovalProcess(token, approval);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "ApprovalController.Reject", 500, "Internal Server Error");
            }
        }
    }
}
