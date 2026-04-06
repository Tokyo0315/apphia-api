using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.Model.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SystemSetup;

namespace Apphia_Website_API.Repository.Service.SystemSetup {
    public class WorkflowService : IWorkflowService {
        private readonly DatabaseContext _context;
        public WorkflowService(DatabaseContext context) { _context = context; }

        public async Task<Workflow> Create(WorkflowCreateViewModel vm, int userId) {
            var entity = new Workflow { WorkflowName = vm.WorkflowName, Description = vm.Description, ApprovalType = vm.ApprovalType, Status = 1, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.Workflows.Add(entity);
            await _context.SaveChangesAsync();

            // Save approvers if provided
            if (vm.WorkflowApprovers != null && vm.WorkflowApprovers.Count > 0) {
                foreach (var a in vm.WorkflowApprovers) {
                    _context.WorkflowApprovers.Add(new WorkflowApprover {
                        WorkflowId = entity.Id, ApproverName = a.ApproverName, EmailAddress = a.EmailAddress,
                        ApprovalOrder = a.ApprovalOrder, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<List<Workflow>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.Workflows.Where(w => w.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(w => w.WorkflowName.Contains(filter));
            return await query.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<WorkflowReadViewModel> ReadSF(int id) {
            var entity = await _context.Workflows.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            var approvers = await _context.WorkflowApprovers.Where(a => a.WorkflowId == id && a.IsActive == true).OrderBy(a => a.ApprovalOrder)
                .Select(a => new WorkflowApproverCreateViewModel { ApproverName = a.ApproverName, EmailAddress = a.EmailAddress, ApprovalOrder = a.ApprovalOrder }).ToListAsync();
            return new WorkflowReadViewModel { Id = entity.Id, WorkflowName = entity.WorkflowName, Description = entity.Description, Status = entity.Status.ToString(), ApprovalType = entity.ApprovalType, WorkflowApprovers = approvers };
        }

        public async Task<string> Update(WorkflowUpdateViewModel vm, int id, int userId) {
            var entity = await _context.Workflows.FindAsync(id);
            if (entity == null) throw new NotFoundResource();
            entity.WorkflowName = vm.WorkflowName; entity.Description = vm.Description; entity.ApprovalType = vm.ApprovalType;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            // Save approvers if provided
            if (vm.WorkflowApprovers != null && vm.WorkflowApprovers.Count > 0) {
                var existing = await _context.WorkflowApprovers.Where(a => a.WorkflowId == id).ToListAsync();
                _context.WorkflowApprovers.RemoveRange(existing);
                foreach (var a in vm.WorkflowApprovers) {
                    _context.WorkflowApprovers.Add(new WorkflowApprover {
                        WorkflowId = id, ApproverName = a.ApproverName, EmailAddress = a.EmailAddress,
                        ApprovalOrder = a.ApprovalOrder, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();
            }

            return "Updated Successfully";
        }

        public async Task<bool> Delete(int id, int userId) {
            var entity = await _context.Workflows.FindAsync(id);
            if (entity == null) return false;
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Restore(int id, int userId) {
            var entity = await _context.Workflows.FindAsync(id);
            if (entity == null) return false;
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public class WorkflowApproverService : IWorkflowApproverService {
        private readonly DatabaseContext _context;
        public WorkflowApproverService(DatabaseContext context) { _context = context; }

        public async Task<bool> WorkflowApprover(int workflowId, string approvalType, List<WorkflowApproverCreateViewModel> viewModel, int userId) {
            var existing = await _context.WorkflowApprovers.Where(a => a.WorkflowId == workflowId).ToListAsync();
            _context.WorkflowApprovers.RemoveRange(existing);
            foreach (var vm in viewModel) {
                _context.WorkflowApprovers.Add(new WorkflowApprover { WorkflowId = workflowId, ApproverName = vm.ApproverName, EmailAddress = vm.EmailAddress, ApprovalOrder = vm.ApprovalOrder, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now });
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<WorkflowApproverCreateViewModel>> ReadSF(int workflowId) {
            return await _context.WorkflowApprovers.Where(a => a.WorkflowId == workflowId && a.IsActive == true).OrderBy(a => a.ApprovalOrder)
                .Select(a => new WorkflowApproverCreateViewModel { ApproverName = a.ApproverName, EmailAddress = a.EmailAddress, ApprovalOrder = a.ApprovalOrder }).ToListAsync();
        }
    }
}
