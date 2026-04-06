using Apphia_Website_API.Repository.Model.Core;
using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Repository.Interface.Core {
    public interface IEmployeeService {
        Task<Employee?> GetById(int employeeId);
        Task<EmployeeReadViewModel> ReadEmployee(int employeeId);
        Task<EmployeeUpdateViewModel> EditEmployee(EmployeeUpdateViewModel employeeUpdateViewModel, int employeeId, int userId);
    }
}
