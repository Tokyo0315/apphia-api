using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.Model.Core;
using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Repository.Service.Core {
    public class ApprovalService : IApprovalService {
        private readonly DatabaseContext _context;
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;
        private readonly ISectionFormattingService _sectionFormattingService;

        public ApprovalService(DatabaseContext context, IApiService apiService, IConfiguration configuration, ISectionFormattingService sectionFormattingService) {
            _context = context;
            _apiService = apiService;
            _configuration = configuration;
            _sectionFormattingService = sectionFormattingService;
        }

        public async Task<ApprovalViewModel> GetToken(string token) {
            object? content = null;
            string? title = null;
            string? emailAddress = null;

            var transaction = await _context.Approvals.FirstOrDefaultAsync(x => x.Token == token && x.IsActive == true);
            if (transaction == null) throw new NotFoundResource("Token Not Found");
            if (transaction.ExpirationDate < DateTime.Now) throw new NotFoundResource("Token Expired");

            if (transaction.Module == "SectionFormatting") {
                // TransactionId = SectionFormattingVersionWorkflowApprovers.Id
                var currentVWA = await _context.SectionFormattingVersionWorkflowApprovers
                    .Where(x => x.Id == transaction.TransactionId && x.ApprovalStatus == "Pending")
                    .FirstOrDefaultAsync();
                if (currentVWA == null) throw new NotFoundResource("Approver not found");

                var currentVW = await _context.SectionFormattingVersionWorkflows
                    .Where(x => x.Id == currentVWA.SectionFormattingVersionWorkflowId)
                    .FirstOrDefaultAsync();
                if (currentVW == null) throw new NotFoundResource("Workflow not found");

                var currentV = await _context.SectionFormattingVersions
                    .Where(x => x.Id == currentVW.SectionFormattingVersionId)
                    .FirstOrDefaultAsync();
                if (currentV == null) throw new NotFoundResource("Version not found");

                var current = await _context.SectionFormattings
                    .Where(x => x.Id == currentV.SectionFormattingId)
                    .FirstOrDefaultAsync();
                if (current == null) throw new NotFoundResource("Section Formatting not found");

                title = current.Name;
                emailAddress = currentVWA.EmailAddress;
                content = new {
                    Html = currentV.Html,
                    Css = currentV.Css,
                    Js = currentV.Js
                };
            }

            return new ApprovalViewModel {
                Token = transaction.Token,
                Module = transaction.Module,
                TransactionId = transaction.TransactionId,
                ExpirationDate = transaction.ExpirationDate,
                Title = title,
                EmailAddress = emailAddress,
                Content = content
            };
        }

        public async Task<string> ApprovalProcess(string token, ApprovalProcessViewModel approval) {
            var transaction = await _context.Approvals.FirstOrDefaultAsync(x => x.Token == token);
            if (transaction == null) throw new NotFoundResource("Token Not Found");
            if (transaction.ExpirationDate < DateTime.Now) throw new NotFoundResource("Token Expired");

            // Deactivate this token
            transaction.IsActive = false;
            _context.Approvals.Update(transaction);

            if (transaction.Module == "SectionFormatting") {
                return await ProcessSectionFormatting(transaction, approval);
            }

            return approval.IsApprove ? "Approved" : "Rejected";
        }

        private async Task<string> ProcessSectionFormatting(Approval transaction, ApprovalProcessViewModel approval) {
            // TransactionId = approver row ID
            var currentVWA = await _context.SectionFormattingVersionWorkflowApprovers
                .Where(x => x.Id == transaction.TransactionId)
                .FirstOrDefaultAsync();
            if (currentVWA == null) throw new NotFoundResource("Approver not found");

            // Update approver status
            currentVWA.ApprovalStatus = approval.IsApprove ? "Approved" : "Disapproved";
            currentVWA.ApprovalDate = DateTime.Now;

            var currentVW = await _context.SectionFormattingVersionWorkflows
                .Where(x => x.Id == currentVWA.SectionFormattingVersionWorkflowId)
                .FirstOrDefaultAsync();
            if (currentVW == null) throw new NotFoundResource("Workflow not found");

            var currentV = await _context.SectionFormattingVersions
                .Where(x => x.Id == currentVW.SectionFormattingVersionId)
                .FirstOrDefaultAsync();
            if (currentV == null) throw new NotFoundResource("Version not found");

            var current = await _context.SectionFormattings
                .Where(x => x.Id == currentV.SectionFormattingId)
                .FirstOrDefaultAsync();
            if (current == null) throw new NotFoundResource("Section Formatting not found");

            if (approval.IsApprove) {
                await _context.SaveChangesAsync();
                // Send to next approver (Remind with false = route to next)
                await _sectionFormattingService.Remind(current.Id, currentVWA.CreatedByUserId ?? 0, false);
            } else {
                // Disapproved
                currentV.ApprovalStatus = "Disapproved";
                currentV.UpdatedDate = DateTime.Now;

                // Mark all remaining pending approvers as disapproved
                var allVWA = await _context.SectionFormattingVersionWorkflowApprovers
                    .Where(x => x.SectionFormattingVersionWorkflowId == currentVW.Id)
                    .ToListAsync();
                foreach (var a in allVWA.Where(x => x.ApprovalStatus == "Pending")) {
                    a.ApprovalStatus = "Disapproved";
                }

                // Take off workflow
                current.IsOnWorkflow = false;
                await _context.SaveChangesAsync();

                // Get makers (users with SectionFormatting update permission) and email them
                var makers = await _context.UserAccounts.Where(u => u.IsActive == true).ToListAsync();
                foreach (var maker in makers) {
                    await _apiService.Send(maker.UserID!, "approval-rejected", new[] {
                        new { Name = "module", Value = "Section Formatting" },
                        new { Name = "page", Value = current.Name! },
                        new { Name = "name", Value = currentVWA.ApproverName! },
                        new { Name = "email", Value = currentVWA.EmailAddress! }
                    });
                }
            }

            return approval.IsApprove ? "Approved" : "Disapproved";
        }
    }
}
