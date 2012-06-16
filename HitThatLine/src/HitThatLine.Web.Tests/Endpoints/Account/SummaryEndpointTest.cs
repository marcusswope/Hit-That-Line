using AutoMapper;
using HitThatLine.Web.Endpoints.Account;
using HitThatLine.Web.Endpoints.Account.Models;
using Moq;
using NUnit.Framework;
using HitThatLine.Web.Tests.Utility;

namespace HitThatLine.Web.Tests.Endpoints.Account
{
    [TestFixture]
    public class SummaryEndpointTest
    {
        [Test]
        public void GET_MapsRequestToViewModel()
        {
            var mappingEngine = new Mock<IMappingEngine>();
            var endpoint = new SummaryEndpoint(mappingEngine.Object);
            var request = new SummaryRequest();
            var expectedViewModel = new SummaryViewModel();

            mappingEngine.Setup(x => x.Map<SummaryRequest, SummaryViewModel>(request)).Returns(expectedViewModel);
            var viewModel = endpoint.Summary(request);
            viewModel.ShouldEqual(expectedViewModel);
        }
    }
}