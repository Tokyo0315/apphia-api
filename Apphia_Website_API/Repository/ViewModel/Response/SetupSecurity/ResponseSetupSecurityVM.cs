using Apphia_Website_API.Repository.Model.UserManagement;

namespace Apphia_Website_API.Repository.ViewModel.Response.SetupSecurity
{
    public class ResponseSetupSecurityVM : ResponseApiViewModel
    {
        public SetupSecurityManagement Data { get; set; } = new SetupSecurityManagement();
    }
}
