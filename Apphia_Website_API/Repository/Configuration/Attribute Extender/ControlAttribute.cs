using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Repository.Configuration.Attribute_Extender {
    public class ControlAttribute : TypeFilterAttribute {
        public ControlAttribute(AccessType accessType, Policies policy) : base(typeof(ControlFilter)) {
            Arguments = new object[] { accessType, policy };
        }
    }
}
