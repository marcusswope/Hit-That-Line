using FubuMVC.Core;

namespace HitThatLine.Endpoints.Account.Models
{
    public class DuplicateUsernameCommand
    {
        public string Username { get; set; }
    }

    public class DuplicateUsernameResponse : JsonMessage
    {
        public bool IsValid { get; set; }
    }
}