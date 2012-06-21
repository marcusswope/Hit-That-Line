using AutoMapper;
using HitThatLine.Endpoints.Thread;
using HitThatLine.Endpoints.Thread.Models;
using Moq;
using NUnit.Framework;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Endpoints.Thread
{
    public class NewThreadEndpointTest
    {
        [TestFixture]
        public class Get
        {
            [Test]
            public void MapsToViewModel()
            {
                var endpoint = TestableNewThreadEndpoint.Build();
                var request = new NewThreadRequest();
                var expectedViewModel = new NewThreadViewModel();

                endpoint.Mapper.Setup(x => x.Map<NewThreadRequest, NewThreadViewModel>(request)).Returns(expectedViewModel);

                var viewModel = endpoint.NewThread(request);

                viewModel.ShouldEqual(expectedViewModel);
            }
        }

        public class TestableNewThreadEndpoint : NewThreadEndpoint
        {
            public Mock<IMappingEngine> Mapper { get; private set; }

            public TestableNewThreadEndpoint(Mock<IMappingEngine> mapper)
                : base(mapper.Object)
            {
                Mapper = mapper;
            }

            public static TestableNewThreadEndpoint Build()
            {
                return new TestableNewThreadEndpoint(new Mock<IMappingEngine>());
            }
        }
    }
}