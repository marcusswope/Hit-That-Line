using FubuMVC.Core;

namespace HitThatLine.Endpoints.Account.Models
{
    public class DuplicateEmailAddressCommand
    {
        public string EmailAddress { get; set; }
    }

    public class DuplicateEmailAddressResponse : JsonMessage
    {
        public bool IsValid { get; set; }
    }
}