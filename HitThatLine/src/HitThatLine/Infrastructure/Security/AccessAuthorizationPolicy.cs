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
        private readonly HttpContextBase _httpContext;
        private readonly ICookieStorage _cookieStorage;
        private readonly IDocumentSession _session;

        public AccessAuthorizationPolicy(HttpContextBase httpContext, ICookieStorage cookieStorage, IDocumentSession session)
        {
            _httpContext = httpContext;
            _cookieStorage = cookieStorage;
            _session = session;
        }

        public AuthorizationRight RightsFor(IFubuRequest request)
        {
            var account = getAccount();
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

        private UserAccount getAccount()
        {
            var principal = _httpContext.User as HTLPrincipal;
            if (principal != null)
            {
                return principal.UserAccount;
            }
            if (_cookieStorage.Contains(UserAccount.LoginCookieName))
            {
                var userKey = _cookieStorage.Get(UserAccount.LoginCookieName);
                var userAccount = _session.Load<UserAccount>(userKey);
                _httpContext.User = userAccount.Principal;
                return userAccount;
            }
            return null;
        }
    }
}