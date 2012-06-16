using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Services;
using Moq;
using NUnit.Framework;
using HitThatLine.Web.Tests.Utility;

namespace HitThatLine.Web.Tests.Endpoints.Account
{
    [TestFixture]
    public class LogoutEndpointTest
    {
        [Test]
        public void LogsOutAndRedirects()
        {
            var service = new Mock<IUserAccountService>();
            var endpoint = new LogoutEndpoint(service.Object);
            var request = new LogoutRequest { UserAccount = new UserAccount() };

            var continuation = endpoint.Logout(request);
            service.Verify(x => x.Logout(request.UserAccount));
            continuation.AssertWasRedirectedTo<HomeInputModel>();
        }
    }
}