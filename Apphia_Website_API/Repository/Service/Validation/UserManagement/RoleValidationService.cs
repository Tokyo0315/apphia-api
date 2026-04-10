using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Interface.Validation.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;
using System.Data;

namespace Apphia_Website_API.Repository.Service.Validation.UserManagement
{
    public class RoleValidationService : IRoleValidationService
    {
        private readonly DatabaseContext _context;

        public RoleValidationService(DatabaseContext context)
        {
            _context = context;
        }

        public bool RoleCreateValidation(RoleCreateViewModel viewModel)
        {
            if (viewModel == null) throw new Exception("Role object is empty");
            if (string.IsNullOrWhiteSpace(viewModel.Code)) throw new Exception("Code is required");
            if (string.IsNullOrWhiteSpace(viewModel.Name)) throw new Exception("Name is required");
            return true;
        }

        public bool RoleUpdateValidation(RoleUpdateViewModel viewModel, int roleId)
        {
            bool duplicate = _context.Roles.Any(r => (r.Code == viewModel.Code) && r.Id != roleId);
            if (duplicate) throw new DuplicateNameException("Code already exists");
            if (string.IsNullOrWhiteSpace(viewModel.Code)) throw new Exception("Code is required");
            if (string.IsNullOrWhiteSpace(viewModel.Name)) throw new Exception("Name is required");
            return true;
        }
    }
}
