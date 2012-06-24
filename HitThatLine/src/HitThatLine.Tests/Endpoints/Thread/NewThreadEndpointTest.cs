using AutoMapper;
using HitThatLine.Domain.Accounts;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Endpoints.Thread;
using HitThatLine.Endpoints.Thread.Models;
using Moq;
using NUnit.Framework;
using HitThatLine.Tests.Utility;
using Raven.Client;

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

        [TestFixture]
        public class Post
        {
            [Test]
            public void CreatesNewThreadAndSavesIt()
            {
                var endpoint = TestableNewThreadEndpoint.Build();
                var command = new NewThreadCommand { Tags = "test,tags", TagInput = "hogs baseball", Author = new UserAccount() };
                var request = new ViewThreadRequest();

                endpoint.Mapper
                    .Setup(x => x.Map<DiscussionThread, ViewThreadRequest>(It.IsAny<DiscussionThread>()))
                    .Returns(request);

                var continuation = endpoint.NewThread(command);

                endpoint.Session.Verify(x => x.Store(It.IsAny<DiscussionThread>()));
                continuation.AssertWasRedirectedTo<ViewThreadRequest>();
            }
        }


        public class TestableNewThreadEndpoint : NewThreadEndpoint
        {
            public Mock<IMappingEngine> Mapper { get; private set; }
            public Mock<IDocumentSession> Session { get; private set; }

            public TestableNewThreadEndpoint(TestableMappingEngine mapper, Mock<IDocumentSession> session)
                : base(mapper, session.Object)
            {
                Session = session;
                Mapper = mapper.Mapper;
            }

            public static TestableNewThreadEndpoint Build()
            {
                return new TestableNewThreadEndpoint(new TestableMappingEngine(), new Mock<IDocumentSession>());
            }
        }
    }
}