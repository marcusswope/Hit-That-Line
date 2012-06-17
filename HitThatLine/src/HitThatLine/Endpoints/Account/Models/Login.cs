using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.Conventions.Attributes;
using HitThatLine.Services;
using HitThatLine.Utility;

namespace HitThatLine.Endpoints.Account.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public LoginRequest()
        { }

        public LoginRequest(LoginCommand input)
        {
            Username = input.Username;
        }
    }

    public class LoginViewModel : LoginRequest
    { }

    public class LoginCommand : LoginViewModel, IValidatedCommand
    {
        public object TransferToOnFailed
        {
            get { return this.MapTo<LoginRequest>(); }
        }

        public UserAccount UserAccount { get; set; }
        public ICookieStorage Cookies { get; set; }
    }
}