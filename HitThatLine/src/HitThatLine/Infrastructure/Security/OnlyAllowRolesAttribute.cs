using System;

namespace HitThatLine.Infrastructure.Security
{
    public class OnlyAllowRolesAttribute : Attribute
    {
        public string[] Roles { get; private set; }

        public OnlyAllowRolesAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
}