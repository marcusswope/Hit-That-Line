using FubuMVC.Core.Runtime;
using HitThatLine.Web.Infrastructure.Conventions.Attributes;

namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string EmailAddress { get; set; }

        public IFubuRequest Request { get; set; }
    }
}