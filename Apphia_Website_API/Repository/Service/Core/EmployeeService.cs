using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.Model.Core;
using Apphia_Website_API.Repository.ViewModel.Core;
using System.Text.Json;

namespace Apphia_Website_API.Repository.Service.Core {
    public class EmployeeService : IEmployeeService {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtHelper _jwtHelper;

        public EmployeeService(DatabaseContext context, IHttpContextAccessor httpContextAccessor, IJwtHelper jwtHelper) {
            _context = context; _httpContextAccessor = httpContextAccessor; _jwtHelper = jwtHelper;
        }

        public async Task<Employee?> GetById(int employeeId) => await _context.Employees.FindAsync(employeeId);

        public async Task<EmployeeReadViewModel> ReadEmployee(int employeeId) {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null) throw new NotFoundResource();

            var httpContext = _httpContextAccessor.HttpContext;
            var tokenDataString = _jwtHelper.TokenDecoder(httpContext!);
            var tokenData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(tokenDataString);
            var userId = int.Parse(tokenData!["UserId"].GetString()!);
            var user = await _context.UserAccounts.FirstOrDefaultAsync(ua => ua.EmployeeId == employeeId);
            if (user?.Id != userId) throw new NotFoundResource();

            return new EmployeeReadViewModel { EmployeeNumber = employee.EmployeeNumber, LastName = employee.LastName, GivenName = employee.GivenName, MiddleName = employee.MiddleName };
        }

        public async Task<EmployeeUpdateViewModel> EditEmployee(EmployeeUpdateViewModel vm, int employeeId, int userId) {
            var userEmployee = await _context.UserAccounts.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (userEmployee == null) throw new NotFoundResource();

            var httpContext = _httpContextAccessor.HttpContext;
            var tokenDataString = _jwtHelper.TokenDecoder(httpContext!);
            var tokenData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(tokenDataString);
            var userId2 = int.Parse(tokenData!["UserId"].GetString()!);
            if (userEmployee.Id != userId2) throw new NotFoundResource();

            var employee = await _context.Employees.FindAsync(userEmployee.EmployeeId);
            if (employee == null) throw new NotFoundResource();

            employee.LastName = vm.LastName; employee.GivenName = vm.GivenName; employee.MiddleName = vm.MiddleName;
            await _context.SaveChangesAsync();
            return new EmployeeUpdateViewModel { LastName = employee.LastName, GivenName = employee.GivenName, MiddleName = employee.MiddleName };
        }
    }
}
