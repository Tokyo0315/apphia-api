using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.Model.Core;
using Apphia_Website_API.Repository.Model.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SectionFormatting;

namespace Apphia_Website_API.Repository.Service.SystemSetup {
    public class SectionFormattingService : ISectionFormattingService {
        private readonly DatabaseContext _context;
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public SectionFormattingService(DatabaseContext context, IApiService apiService, IConfiguration configuration) {
            _context = context;
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<SectionFormatting> Create(SectionFormattingCreateViewModel model, int userId) {
            var entity = new SectionFormatting { Tab = model.Tab, Name = model.Name, TabOrder = model.TabOrder, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.SectionFormattings.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<SectionFormattingReadViewModel>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.SectionFormattings.Where(s => s.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(s => s.Name!.Contains(filter) || s.Tab!.Contains(filter));

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(s => s.TabOrder).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();

            return items.Select(s => {
                var latestVersion = _context.SectionFormattingVersions
                    .Where(v => v.SectionFormattingId == s.Id && v.IsActive == true)
                    .OrderByDescending(v => v.CreatedDate)
                    .FirstOrDefault();

                return new SectionFormattingReadViewModel {
                    Id = s.Id,
                    Section = s.Tab ?? "",
                    Description = s.Name ?? "",
                    SequenceOrder = s.TabOrder,
                    ApprovalStatus = latestVersion?.ApprovalStatus ?? (s.IsOnWorkflow == true ? "For Review/Approval" : "Saved"),
                    IsActive = s.IsActive,
                    TotalCount = totalCount,
                    DeletedBy = s.DeletedByUserId != null ? _context.UserAccounts.Include(x => x.Employee).Where(x => x.Id == s.DeletedByUserId).Select(x => x.Employee!.GivenName + " " + x.Employee.LastName).FirstOrDefault() : null,
                    DeletedDate = s.DeletedDate?.ToString("MMMM d, yyyy"),
                };
            }).ToList();
        }

        public async Task<SectionFormatting> ReadSF(int id, bool isActive) {
            var entity = await _context.SectionFormattings.FirstOrDefaultAsync(s => s.Id == id && s.IsActive == isActive);
            if (entity == null) throw new NotFoundResource();
            return entity;
        }

        public async Task<SectionFormattingGrapesJSViewModel> ReadGrapesJS(int id) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            return new SectionFormattingGrapesJSViewModel { Html = entity.Html, Css = entity.Css, Js = entity.Js, Data = entity.Data };
        }

        public async Task<List<SectionFormattingTabs>> ReadTabs() {
            var sections = await _context.SectionFormattings.Where(s => s.IsActive == true).OrderBy(s => s.TabOrder).ToListAsync();
            return sections.Select(s => new SectionFormattingTabs { Label = s.Name ?? "", Route = s.Tab ?? "" }).ToList();
        }

        public async Task<SectionFormattingGrapesJSViewModel> ReadWebsite(string page) {
            var entity = await _context.SectionFormattings.FirstOrDefaultAsync(s => s.Tab == page && s.IsActive == true);
            if (entity == null) throw new NotFoundResource();

            // Only serve content that has been approved and published (Posted)
            var postedVersion = await _context.SectionFormattingVersions
                .Where(v => v.SectionFormattingId == entity.Id && v.ApprovalStatus == "Posted" && v.IsActive == true)
                .OrderByDescending(v => v.CreatedDate)
                .FirstOrDefaultAsync();

            if (postedVersion != null) {
                return new SectionFormattingGrapesJSViewModel { Html = postedVersion.Html, Css = postedVersion.Css, Js = postedVersion.Js };
            }

            // Nothing published yet — public site shows nothing
            return new SectionFormattingGrapesJSViewModel { Html = null, Css = null, Js = null };
        }

        public async Task<string> Update(SectionFormattingUpdateViewModel model, int id, int userId) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.Tab = model.Tab; entity.Name = model.Name; entity.TabOrder = model.TabOrder;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return "Updated Successfully";
        }

        public async Task<bool> UpdateGrapesJS(SectionFormattingGrapesJSViewModel model, int id, int userId) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.Html = model.Html; entity.Css = model.Css; entity.Js = model.Js; entity.Data = model.Data;
            entity.HasContentChanges = true; entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            entity.IsOnWorkflow = false;

            // Deactivate any previous versions so the new "Saved" version becomes the latest
            var previousVersions = await _context.SectionFormattingVersions
                .Where(v => v.SectionFormattingId == id && v.IsActive == true)
                .ToListAsync();
            foreach (var v in previousVersions) v.IsActive = false;

            // Create a new version snapshot with "Saved" status so Route button becomes available
            _context.SectionFormattingVersions.Add(new SectionFormattingVersion {
                SectionFormattingId = id,
                Html = model.Html,
                Css = model.Css,
                Js = model.Js,
                Data = model.Data,
                ApprovalStatus = "Saved",
                IsActive = true,
                CreatedByUserId = userId,
                CreatedDate = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id, int userId) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) return false;
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Restore(int id, int userId) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) return false;
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        // ==================== WORKFLOW ACTIONS ==================== //

        public async Task<int> Route(int id, int workflowId, int userId) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) throw new NotFoundResource("Section Formatting not found");

            var workflow = await _context.Workflows.FindAsync(workflowId);
            if (workflow == null) throw new NotFoundResource("Workflow not found");

            // 1. Create a version snapshot of the current content
            var version = new SectionFormattingVersion {
                SectionFormattingId = id,
                Html = entity.Html,
                Css = entity.Css,
                Js = entity.Js,
                Data = entity.Data,
                ApprovalStatus = "For Review/Approval",
                IsActive = true,
                CreatedByUserId = userId,
                CreatedDate = DateTime.Now
            };
            _context.SectionFormattingVersions.Add(version);
            await _context.SaveChangesAsync();

            // 2. Link version to workflow
            var versionWorkflow = new SectionFormattingVersionWorkflow {
                SectionFormattingVersionId = version.Id,
                WorkflowId = workflowId,
                IsActive = true,
                CreatedByUserId = userId,
                CreatedDate = DateTime.Now
            };
            _context.SectionFormattingVersionWorkflows.Add(versionWorkflow);
            await _context.SaveChangesAsync();

            // 3. Copy workflow approvers to version workflow approvers
            var approvers = await _context.WorkflowApprovers
                .Where(a => a.WorkflowId == workflowId && a.IsActive == true)
                .OrderBy(a => a.ApprovalOrder)
                .ToListAsync();

            var versionApproverIds = new List<SectionFormattingVersionWorkflowApprover>();
            foreach (var approver in approvers) {
                var versionApprover = new SectionFormattingVersionWorkflowApprover {
                    SectionFormattingVersionWorkflowId = versionWorkflow.Id,
                    ApproverName = approver.ApproverName,
                    EmailAddress = approver.EmailAddress,
                    ApprovalOrder = approver.ApprovalOrder,
                    ApprovalStatus = "Pending",
                    IsActive = true,
                    CreatedByUserId = userId,
                    CreatedDate = DateTime.Now
                };
                _context.SectionFormattingVersionWorkflowApprovers.Add(versionApprover);
                versionApproverIds.Add(versionApprover);
            }
            await _context.SaveChangesAsync();

            // 4. Mark section formatting as on workflow
            entity.IsOnWorkflow = true;
            await _context.SaveChangesAsync();

            // 5. Create approval tokens and send emails to approvers
            var approvalBaseUrl = _configuration["FrontEndDomains:Email"] ?? "http://localhost:4201/Approval/";

            // For Serial, only email first approver; for Parallel, email all
            var approversToNotify = workflow.ApprovalType == "Serial"
                ? versionApproverIds.OrderBy(a => a.ApprovalOrder).Take(1).ToList()
                : versionApproverIds;

            foreach (var vwa in approversToNotify) {
                // TransactionId = the version approver row ID (matches SSCGI pattern)
                var approval = new Approval {
                    Module = "SectionFormatting",
                    TransactionId = vwa.Id,
                    IsActive = true,
                    CreatedByUserId = userId,
                    CreatedDate = DateTime.Now
                };
                _context.Approvals.Add(approval);
                await _context.SaveChangesAsync();

                var approvalLink = $"{approvalBaseUrl}{approval.Token}";

                var emailBody = new[] {
                    new { Name = "module", Value = "Section Formatting" },
                    new { Name = "page", Value = entity.Name ?? "" },
                    new { Name = "name", Value = vwa.ApproverName ?? "" },
                    new { Name = "email", Value = vwa.EmailAddress ?? "" },
                    new { Name = "expirationDate", Value = approval.ExpirationDate.ToString("MMMM dd, yyyy") },
                    new { Name = "approvalLink", Value = approvalLink }
                };

                await _apiService.Send(vwa.EmailAddress!, "approval-remind", emailBody);
            }

            return version.Id;
        }

        public async Task<string> Remind(int id, int userId, bool remind) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) throw new NotFoundResource("Section Formatting not found");

            var latestVersion = await _context.SectionFormattingVersions
                .Where(v => v.SectionFormattingId == id)
                .OrderByDescending(v => v.Id)
                .FirstOrDefaultAsync();
            if (latestVersion == null) throw new NotFoundResource("No version found");

            var versionWorkflow = await _context.SectionFormattingVersionWorkflows
                .Where(vw => vw.SectionFormattingVersionId == latestVersion.Id)
                .FirstOrDefaultAsync();
            if (versionWorkflow == null) throw new NotFoundResource("No workflow found");

            // Get pending approvers (not yet approved/rejected)
            var pendingAll = await _context.SectionFormattingVersionWorkflowApprovers
                .Where(a => a.SectionFormattingVersionWorkflowId == versionWorkflow.Id && a.ApprovalStatus == "Pending")
                .OrderBy(a => a.ApprovalOrder)
                .ToListAsync();

            // Only notify the current order level (lowest pending order)
            var currentOrder = pendingAll.Any() ? pendingAll.Min(x => x.ApprovalOrder) : null;
            var approvers = currentOrder != null
                ? pendingAll.Where(x => x.ApprovalOrder == currentOrder).ToList()
                : new List<SectionFormattingVersionWorkflowApprover>();

            var approvalBaseUrl = _configuration["FrontEndDomains:Email"] ?? "http://localhost:4201/Approval/";

            foreach (var approver in approvers) {
                // Check if existing approval token exists and is still valid
                var existingApproval = await _context.Approvals
                    .Where(x => x.Module == "SectionFormatting" && x.TransactionId == approver.Id)
                    .FirstOrDefaultAsync();

                if (existingApproval == null || existingApproval.ExpirationDate < DateTime.Now) {
                    if (existingApproval != null) {
                        existingApproval.IsActive = false;
                        _context.Approvals.Update(existingApproval);
                    }
                    existingApproval = new Approval {
                        Module = "SectionFormatting",
                        TransactionId = approver.Id,
                        IsActive = true,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now
                    };
                    _context.Approvals.Add(existingApproval);
                }

                await _context.SaveChangesAsync();

                var approvalLink = $"{approvalBaseUrl}{existingApproval.Token}";

                await _apiService.Send(approver.EmailAddress!, "approval-remind", new[] {
                    new { Name = "module", Value = "Section Formatting" },
                    new { Name = "page", Value = entity.Name ?? "" },
                    new { Name = "name", Value = approver.ApproverName ?? "" },
                    new { Name = "email", Value = approver.EmailAddress ?? "" },
                    new { Name = "expirationDate", Value = existingApproval.ExpirationDate.ToString("MMMM dd, yyyy") },
                    new { Name = "approvalLink", Value = approvalLink }
                });
            }

            // If no more pending approvers → Fully Approved
            if (approvers.Count == 0) {
                latestVersion.ApprovalStatus = "Fully Approved";
                latestVersion.UpdatedByUserId = userId;
                latestVersion.UpdatedDate = DateTime.Now;

                entity.IsOnWorkflow = false;
                await _context.SaveChangesAsync();

                // Email all admin users that content is fully approved
                var makers = await _context.UserAccounts.Where(u => u.IsActive == true).ToListAsync();
                foreach (var maker in makers) {
                    await _apiService.Send(maker.UserID!, "approval-fully-approved", new[] {
                        new { Name = "module", Value = "Section Formatting" },
                        new { Name = "page", Value = entity.Name ?? "" }
                    });
                }

                return "Fully Approved";
            }

            return "Reminder sent";
        }

        public async Task<string> Recall(int id, int userId) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) throw new NotFoundResource("Section Formatting not found");

            // Get the latest active version
            var latestVersion = await _context.SectionFormattingVersions
                .Where(v => v.SectionFormattingId == id && v.IsActive == true)
                .OrderByDescending(v => v.CreatedDate)
                .FirstOrDefaultAsync();

            if (latestVersion != null) {
                latestVersion.ApprovalStatus = "Recalled";

                // Get version workflow and deactivate pending approvers
                var versionWorkflow = await _context.SectionFormattingVersionWorkflows
                    .Where(vw => vw.SectionFormattingVersionId == latestVersion.Id && vw.IsActive == true)
                    .FirstOrDefaultAsync();

                if (versionWorkflow != null) {
                    var pendingApprovers = await _context.SectionFormattingVersionWorkflowApprovers
                        .Where(a => a.SectionFormattingVersionWorkflowId == versionWorkflow.Id
                            && a.ApprovalStatus == "Pending"
                            && a.IsActive == true)
                        .ToListAsync();

                    foreach (var approver in pendingApprovers) {
                        approver.ApprovalStatus = "Recalled";
                    }
                }

                // Expire related approval tokens
                var approvalTokens = await _context.Approvals
                    .Where(a => a.Module == "SectionFormatting" && a.TransactionId == latestVersion.Id && a.ExpirationDate > DateTime.Now)
                    .ToListAsync();

                foreach (var token in approvalTokens) {
                    token.ExpirationDate = DateTime.Now;
                }
            }

            entity.IsOnWorkflow = false;
            await _context.SaveChangesAsync();

            return "Recalled";
        }

        public async Task<string> Publish(int id, int userId) {
            var entity = await _context.SectionFormattings.FindAsync(id);
            if (entity == null) throw new NotFoundResource();

            // Get the latest approved version and apply its content
            var latestVersion = await _context.SectionFormattingVersions
                .Where(v => v.SectionFormattingId == id && v.IsActive == true && v.ApprovalStatus == "Fully Approved")
                .OrderByDescending(v => v.CreatedDate)
                .FirstOrDefaultAsync();

            if (latestVersion != null) {
                entity.Html = latestVersion.Html;
                entity.Css = latestVersion.Css;
                entity.Js = latestVersion.Js;
                entity.Data = latestVersion.Data;
                latestVersion.ApprovalStatus = "Posted";
            }

            entity.HasContentChanges = false;
            entity.IsOnWorkflow = false;
            await _context.SaveChangesAsync();

            return "Published";
        }

        // ==================== APPROVAL STATUS ==================== //

        public async Task<object> ReadApprovalStatus(int id) {
            var latestVersion = await _context.SectionFormattingVersions
                .Where(v => v.SectionFormattingId == id && v.IsActive == true)
                .OrderByDescending(v => v.CreatedDate)
                .FirstOrDefaultAsync();

            if (latestVersion == null) return new { approvalStatus = "Saved", approvers = Array.Empty<object>() };

            var versionWorkflow = await _context.SectionFormattingVersionWorkflows
                .Where(vw => vw.SectionFormattingVersionId == latestVersion.Id && vw.IsActive == true)
                .FirstOrDefaultAsync();

            if (versionWorkflow == null) return new { approvalStatus = latestVersion.ApprovalStatus, approvers = Array.Empty<object>() };

            var approvers = await _context.SectionFormattingVersionWorkflowApprovers
                .Where(a => a.SectionFormattingVersionWorkflowId == versionWorkflow.Id && a.IsActive == true)
                .OrderBy(a => a.ApprovalOrder)
                .Select(a => new {
                    approverName = a.ApproverName,
                    emailAddress = a.EmailAddress,
                    approvalOrder = a.ApprovalOrder,
                    approvalStatus = a.ApprovalStatus,
                    approvalDate = a.ApprovalDate
                })
                .ToListAsync();

            return new {
                approvalStatus = latestVersion.ApprovalStatus,
                approvers
            };
        }
    }
}
