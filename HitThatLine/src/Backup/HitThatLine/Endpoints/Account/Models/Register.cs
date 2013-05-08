using System.Web;
using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.Validation;
using HitThatLine.Infrastructure.Validation.Attributes;
using HitThatLine.Services;

namespace HitThatLine.Endpoints.Account.Models
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string EmailAddress { get; set; }
    }

    public class RegisterViewModel : RegisterRequest
    { }

    public class RegisterCommand : RegisterViewModel, IValidatedCommand
    {
        public ICookieStorage Cookies { get; set; }
        public HttpContextBase HttpContext { get; set; }
        public object TransferToOnFailed
        {
            get { return new RegisterRequest(); }
        }
    }
}