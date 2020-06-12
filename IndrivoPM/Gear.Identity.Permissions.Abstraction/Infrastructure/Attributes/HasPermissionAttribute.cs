using System;
using Microsoft.AspNetCore.Authorization;

namespace Gear.Identity.Permissions.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Domain.Resources.Permissions permission) : base(permission.ToString())
        { }
    }
}
