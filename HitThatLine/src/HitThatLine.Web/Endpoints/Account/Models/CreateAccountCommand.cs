namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class CreateAccountCommand
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}