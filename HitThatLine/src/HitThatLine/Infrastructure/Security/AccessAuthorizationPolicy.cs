using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using HitThatLine.Domain.Accounts;
using HitThatLine.Services;
using Raven.Client;

namespace HitThatLine.Infrastructure.Security
{
    public class AccessAuthorizationPolicy<THandler> : IAuthorizationPolicy
    {
        private readonly IUserAccountService _service;

        public AccessAuthorizationPolicy(IUserAccountService service)
        {
            _service = service;
        }

        public AuthorizationRight RightsFor(IFubuRequest request)
        {
            var account = _service.GetCurrent();
            if (account == null) return AuthorizationRight.Deny;

            var requiredRoles = getRequiredRoles();

            if (!requiredRoles.All(account.Roles.Contains))
            {
                return AuthorizationRight.Deny;
            }

            return AuthorizationRight.Allow;
        }

        private IEnumerable<string> getRequiredRoles()
        {
            return typeof (THandler)
                .GetCustomAttributes(false)
                .Where(x => x is OnlyAllowRolesAttribute)
                .Cast<OnlyAllowRolesAttribute>()
                .SelectMany(x => x.Roles).ToList();
        }
    }
}