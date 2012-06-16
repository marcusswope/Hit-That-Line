using HitThatLine.Web.Infrastructure;
using HitThatLine.Web.Infrastructure.Conventions.Attributes;

namespace HitThatLine.Web.Endpoints.Account.Models
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

        public RegisterRequest()
        {

        }

        public RegisterRequest(RegisterRequest command)
        {
            Username = command.Username;
            EmailAddress = command.EmailAddress;
        }
    }

    public class RegisterViewModel : RegisterRequest
    {
        public RegisterViewModel()
        {

        }

        public RegisterViewModel(RegisterRequest request)
            : base(request)
        {

        }
    }

    public class RegisterCommand : RegisterViewModel, IValidatedCommand
    {
        public object TransferToOnFailed
        {
            get { return new RegisterRequest(this); }
        }
    }
}