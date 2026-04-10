using Microsoft.AspNetCore.Mvc.Filters;
using Apphia_Website_API.Repository.Configuration.Enum;
using Apphia_Website_API.Repository.Interface.UserManagement;

namespace Apphia_Website_API.Repository.Configuration.Attribute_Extender
{
    public class ControlFilter : IAsyncActionFilter
    {
        private readonly IRolePolicyControlService _rpcService;
        public Policies _policy { get; set; }
        public AccessType _accessType { get; set; }

        public ControlFilter(IRolePolicyControlService rpcService, AccessType accessType, Policies policy)
        {
            _rpcService = rpcService;
            _accessType = accessType;
            _policy = policy;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            var roleId = Convert.ToInt32(user.Claims.FirstOrDefault(s => s.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value);

            if (roleId <= 0) throw new UnauthorizedAccessException("User is not authenticated.");

            var pcList = await _rpcService.GetPolicyControlById((int)_policy, roleId);
            foreach (var List in pcList)
            {
                if (List.Id == (int)_policy)
                {
                    var accessName = _accessType.ToString();
                    var controlProperty = List.Controls.GetType().GetProperty(accessName);

                    if (List.Controls != null)
                    {
                        var value = controlProperty.GetValue(List.Controls);
                        if (value is bool hasAccess && hasAccess)
                        {
                            await next();
                            return;
                        }
                        else throw new UnauthorizedAccessException($"User does not have {accessName} access for policy {_policy}.");
                    }
                    else throw new UnauthorizedAccessException($"User does not have {accessName} access for policy {_policy}.");
                }
            }

            throw new UnauthorizedAccessException("User is not authenticated.");
        }
    }
}
