using System.Web;
using FubuMVC.Core.Behaviors;
using HitThatLine.Domain.Accounts;
using HitThatLine.Services;
using Raven.Client;

namespace HitThatLine.Infrastructure.Behaviors
{
    public class UserAccountBehavior : IActionBehavior
    {
        private readonly ICookieStorage _cookieStorage;
        private readonly IDocumentSession _session;
        private readonly HttpContextBase _httpContext;

        public UserAccountBehavior(ICookieStorage cookieStorage, IDocumentSession session, HttpContextBase httpContext)
        {
            _cookieStorage = cookieStorage;
            _session = session;
            _httpContext = httpContext;
        }

        public IActionBehavior InsideBehavior { get; set; }

        public void Invoke()
        {
            if (_cookieStorage.Contains(UserAccount.LoginCookieName))
            {
                var userKey = _cookieStorage.Get(UserAccount.LoginCookieName);
                var user = _session.Load<UserAccount>(userKey);
                _httpContext.User = user.Principal;
            }

            InsideBehavior.Invoke();
        }

        public void InvokePartial()
        {
            InsideBehavior.InvokePartial();
        }
    }
}