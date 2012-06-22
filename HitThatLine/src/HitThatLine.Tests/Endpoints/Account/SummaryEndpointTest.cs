using AutoMapper;
using HitThatLine.Endpoints.Account;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Endpoints.Account
{
    [TestFixture]
    public class SummaryEndpointTest
    {
        [TestFixture]
        public class Get
        {
            [Test]
            public void MapsRequestToViewModel()
            {
                var mappingEngine = new TestableMappingEngine();
                var endpoint = new SummaryEndpoint(mappingEngine);
                var request = new SummaryRequest();
                var expectedViewModel = new SummaryViewModel();

                mappingEngine.Mapper.Setup(x => x.Map<SummaryRequest, SummaryViewModel>(request)).Returns(expectedViewModel);
                var viewModel = endpoint.Summary(request);
                viewModel.ShouldEqual(expectedViewModel);
            }
        }
    }
}