using System.Security.Principal;
using HitThatLine.Domain.Accounts;

namespace HitThatLine.Infrastructure.Security
{
    public class HTLPrincipal : GenericPrincipal
    {
        private readonly UserAccount _account;
        public UserAccount UserAccount { get { return _account; } }

        public HTLPrincipal(UserAccount account)
            : base(new GenericIdentity(account.Username), account.Roles.ToArray())
        {
            _account = account;
        }
    }
}