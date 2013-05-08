using HitThatLine.Domain.Accounts;

namespace HitThatLine.Endpoints.Account.Models
{
    public class SummaryRequest
    {
        public bool LoggedIn { get { return User != null; } }
        public UserAccount User { get; set; }
    }

    public class SummaryViewModel
    {
        public string UserUsername { get; set; }
        public bool LoggedIn { get; set; }
        public string UserProfilePictureUrl { get; set; }
    }
}