namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginInputModel()
        { }

        public LoginInputModel(LoginViewModel input)
        {
            Username = input.Username;
        }
    }
}