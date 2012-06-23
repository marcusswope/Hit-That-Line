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
            var service = new Mock<IUserAccountService>();
            var endpoint = new LogoutEndpoint(service.Object);
            var request = new LogoutRequest();

            var continuation = endpoint.Logout(request);
            service.Verify(x => x.Logout());
            continuation.AssertWasRedirectedTo<HomeRequest>();
        }
    }
}