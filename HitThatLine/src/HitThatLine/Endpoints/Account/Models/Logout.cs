using HitThatLine.Domain.Accounts;
using HitThatLine.Services;

namespace HitThatLine.Endpoints.Account.Models
{
    public class LogoutRequest
    {
        public UserAccount UserAccount { get; set; }
        public ICookieStorage Cookies { get; set; }
    }
}