using System;

namespace HitThatLine.Infrastructure.Security
{
    public class OnlyAllowRoleAttribute : Attribute
    {
        public string[] Roles { get; private set; }

        public OnlyAllowRoleAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
}