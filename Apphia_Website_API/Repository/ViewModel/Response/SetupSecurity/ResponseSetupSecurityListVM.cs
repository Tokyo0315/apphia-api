using Apphia_Website_API.Repository.Model.UserManagement;

namespace Apphia_Website_API.Repository.ViewModel.Response.SetupSecurity
{
    public class ResponseSetupSecurityListVM : ResponseApiViewModel
    {
        public List<SetupSecurityManagement> Data { get; set; } = new List<SetupSecurityManagement>();
    }
}
