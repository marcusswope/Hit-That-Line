using HitThatLine.Web.Endpoints.Home;
using HitThatLine.Web.Endpoints.Home.Models;
using NUnit.Framework;
using HitThatLine.Web.Tests.Utility;

namespace HitThatLine.Web.Tests.Endpoints.Home
{
    [TestFixture]
    public class HomeEndpointTest
    {
        [Test]
        public void ReturnsCorrectType()
        {
            var endpoint = new HomeEndpoint();

            endpoint.Home(new HomeInputModel()).ShouldBeOfType<HomeViewModel>();
        }
    }
}