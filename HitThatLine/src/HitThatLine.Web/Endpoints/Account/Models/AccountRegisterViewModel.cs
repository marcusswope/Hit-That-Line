using HitThatLine.Web.Infrastructure.Conventions.Attributes;

namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class AccountRegisterViewModel
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
}