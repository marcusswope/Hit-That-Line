using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.Validation;
using HitThatLine.Infrastructure.Validation.Attributes;
using HitThatLine.Services;

namespace HitThatLine.Endpoints.Account.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginViewModel : LoginRequest
    { }

    public class LoginCommand : LoginViewModel, IValidatedCommand
    {
        public ICookieStorage Cookies { get; set; }
        public HttpContextBase HttpContext { get; set; }

        public object TransferToOnFailed
        {
            get { return new LoginRequest(); }
        }
    }
}