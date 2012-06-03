using HitThatLine.Web.Infrastructure.Conventions.Attributes;

namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public LoginViewModel()
        {
            
        }

        public LoginViewModel(LoginInputModel model)
        {
            Username = model.Username;
        }
    }
}