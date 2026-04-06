using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Repository.Configuration.Attribute_Extender {
    public class ControlAttribute : TypeFilterAttribute {
        public ControlAttribute(AccessType accessType, Policies policy) : base(typeof(ControlFilter)) {
            Arguments = new object[] { accessType, policy };
        }
    }

    public class ControlFilter : Microsoft.AspNetCore.Mvc.Filters.IAsyncActionFilter {
        public Policies _policy { get; set; }
        public AccessType _accessType { get; set; }

        public ControlFilter(AccessType accessType, Policies policy) {
            _accessType = accessType;
            _policy = policy;
        }

        public async Task OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context, Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate next) {
            // TODO: Implement role-based access control (same as SSCGI pattern)
            await next();
        }
    }
}
