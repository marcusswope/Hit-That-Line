using HitThatLine.Endpoints.Home;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Tests.Utility;
using NUnit.Framework;

namespace HitThatLine.Tests.Endpoints.Home
{
    [TestFixture]
    public class HomeEndpointTest
    {
        [Test]
        public void ReturnsCorrectType()
        {
            var endpoint = new HomeEndpoint();

            endpoint.Home(new HomeRequest()).ShouldBeOfType<HomeViewModel>();
        }
    }
}