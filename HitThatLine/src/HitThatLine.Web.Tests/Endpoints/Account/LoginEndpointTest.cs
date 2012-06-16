using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Infrastructure.ModelBinding;
using HitThatLine.Web.Services;
using Moq;
using NUnit.Framework;
using HitThatLine.Web.Tests.Utility;

namespace HitThatLine.Web.Tests.Endpoints.Account
{
    [TestFixture]
    public class LoginEndpointTest
    {
        [Test]
        public void GET_MapsRequestToViewModel()
        {
            var endpoint = buildEndpoint();
            var request = new LoginRequest();
            var expectedViewModel = new LoginViewModel();

            endpoint.MappingEngine.Setup(x => x.Map<LoginRequest, LoginViewModel>(request)).Returns(expectedViewModel);
            var viewModel = endpoint.Login(request);
            viewModel.ShouldEqual(expectedViewModel);
        }

        [Test]
        public void POST_LogsInAndRedirects()
        {
            var endpoint = buildEndpoint();
            var command = new LoginCommand { UserAccount = new UserAccount() };

            var redirect = endpoint.Login(command);
            endpoint.Service.Verify(x => x.Login(command.UserAccount));
            redirect.AssertWasRedirectedTo<HomeInputModel>(x => x != null);
        }

        private static TestableLoginEndpoint buildEndpoint()
        {
            return new TestableLoginEndpoint(new Mock<IUserAccountService>(), new Mock<IMappingEngine>());
        }
    }

    internal class TestableLoginEndpoint : LoginEndpoint
    {
        public Mock<IUserAccountService> Service { get; private set; }
        public Mock<IMappingEngine> MappingEngine { get; private set; }

        public TestableLoginEndpoint(Mock<IUserAccountService> service, Mock<IMappingEngine> mappingEngine)
            : base(service.Object, mappingEngine.Object)
        {
            Service = service;
            MappingEngine = mappingEngine;
        }
    }
}