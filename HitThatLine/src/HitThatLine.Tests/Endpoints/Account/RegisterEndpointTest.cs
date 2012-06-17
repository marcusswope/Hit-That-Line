using AutoMapper;
using HitThatLine.Endpoints.Account;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;
using Raven.Client;

namespace HitThatLine.Tests.Endpoints.Account
{
    [TestFixture]
    public class RegisterEndpointTest
    {
        [Test]
        public void GET_MapRequestToModel()
        {
            var endpoint = TestableRegisterEndpoint.Build(new Mock<IDocumentSession>().Object);
            var request = new RegisterRequest();
            var expectedViewModel = new RegisterViewModel();

            endpoint.Mapper.Setup(x => x.Map<RegisterRequest, RegisterViewModel>(request)).Returns(expectedViewModel);

            var model = endpoint.Register(request);

            model.ShouldEqual(expectedViewModel);
        }

        [Test]
        public void POST_CreateNewAccountAndRedirect()
        {
            var endpoint = TestableRegisterEndpoint.Build(new Mock<IDocumentSession>().Object);
            var command = new RegisterCommand();

            var continuation = endpoint.Register(command);

            endpoint.Service.Verify(x => x.CreateNew(command));
            continuation.AssertWasRedirectedTo<HomeRequest>();
        }

        private class ValidatesDuplicateUsernames : RavenTestBase
        {
            [Test]
            public void ThatExist()
            {
                var endpoint = TestableRegisterEndpoint.Build(Session);
                var command = new DuplicateUsernameCommand {Username = DefaultUser.Username};

                endpoint.ValidateUsername(command).IsValid.ShouldBeFalse();
            }

            [Test]
            public void ThatDontExist()
            {
                var endpoint = TestableRegisterEndpoint.Build(Session);
                var command = new DuplicateUsernameCommand { Username = "someOtherUserName" };

                endpoint.ValidateUsername(command).IsValid.ShouldBeTrue();
            }
        }

        private class ValidatesDuplicateEmails : RavenTestBase
        {
            [Test]
            public void ThatExist()
            {
                var endpoint = TestableRegisterEndpoint.Build(Session);
                var command = new DuplicateEmailAddressCommand { EmailAddress = DefaultUser.EmailAddress };

                endpoint.ValidateEmailAddress(command).IsValid.ShouldBeFalse();
            }

            [Test]
            public void ThatDontExist()
            {
                var endpoint = TestableRegisterEndpoint.Build(Session);
                var command = new DuplicateEmailAddressCommand { EmailAddress = "someOtherUserName" };

                endpoint.ValidateEmailAddress(command).IsValid.ShouldBeTrue();
            }
        }
    }

    public class TestableRegisterEndpoint : RegisterEndpoint
    {
        public Mock<IUserAccountService> Service { get; private set; }
        public Mock<IMappingEngine> Mapper { get; private set; }

        public TestableRegisterEndpoint(Mock<IUserAccountService> service, Mock<IMappingEngine> mapper, IDocumentSession session)
            : base(service.Object, mapper.Object, session)
        {
            Service = service;
            Mapper = mapper;
        }

        public static TestableRegisterEndpoint Build(IDocumentSession session)
        {
            return new TestableRegisterEndpoint(new Mock<IUserAccountService>(), new Mock<IMappingEngine>(), session);
        }
    }
}