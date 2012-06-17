using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Endpoints.Account
{
    [TestFixture]
    public class LogoutEndpointTest
    {
        [Test]
        public void LogsOutAndRedirects()
        {
            var cookieStorage = new Mock<ICookieStorage>();
            var endpoint = new LogoutEndpoint();
            var userAccount = new Mock<UserAccount>();
            var request = new LogoutRequest { UserAccount = userAccount.Object, Cookies = cookieStorage.Object };

            var continuation = endpoint.Logout(request);
            userAccount.Verify(x => x.Logout(cookieStorage.Object));
            continuation.AssertWasRedirectedTo<HomeRequest>();
        }
    }
}