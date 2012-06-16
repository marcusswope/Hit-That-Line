using HitThatLine.Core.Accounts;
using HitThatLine.Web.Infrastructure;
using HitThatLine.Web.Infrastructure.Conventions.Attributes;

namespace HitThatLine.Web.Endpoints.Account.Models
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
    {
        public LoginViewModel()
        {
            
        }

        public LoginViewModel(LoginRequest model)
        {
            Username = model.Username;
        }
    }

    public class LoginCommand : LoginViewModel, IValidatedCommand
    {
        public object TransferToOnFailed
        {
            get { return new LoginRequest(this); }
        }

        public UserAccount UserAccount { get; set; }
    }
}