using FubuMVC.Core;
using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.Conventions.Attributes;
using HitThatLine.Utility;

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
        public object TransferToOnFailed
        {
            get { return this.MapTo<RegisterRequest>(); }
        }
    }
}