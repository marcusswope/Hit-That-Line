using System.Web;
using FubuMVC.Core.Behaviors;
using HitThatLine.Services;

namespace HitThatLine.Infrastructure.Security
{
    public class UserAccountBehavior : IActionBehavior
    {
        private readonly HttpContextBase _httpContext;
        private readonly IUserAccountService _service;

        public UserAccountBehavior(HttpContextBase httpContext, IUserAccountService service)
        {
            _httpContext = httpContext;
            _service = service;
        }

        public IActionBehavior InsideBehavior { get; set; }

        public void Invoke()
        {
            var account = _service.GetCurrent();
            if (account != null) _httpContext.User = account.Principal;
            
            InsideBehavior.Invoke();
        }

        public void InvokePartial()
        {
            InsideBehavior.InvokePartial();
        }
    }
}