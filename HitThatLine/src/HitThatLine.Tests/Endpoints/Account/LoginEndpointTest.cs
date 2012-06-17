using System.Web;
using AutoMapper;
using HitThatLine.Domain.Accounts;
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
    public class LoginEndpointTest
    {
        [Test]
        public void GET_MapsRequestToViewModel()
        {
            var endpoint = TestableLoginEndpoint.Build();
            var request = new LoginRequest();
            var expectedViewModel = new LoginViewModel();

            endpoint.MappingEngine.Setup(x => x.Map<LoginRequest, LoginViewModel>(request)).Returns(expectedViewModel);
            var viewModel = endpoint.Login(request);
            viewModel.ShouldEqual(expectedViewModel);
        }

        [Test]
        public void POST_LogsInAndRedirects()
        {
            var endpoint = TestableLoginEndpoint.Build();
            var userAccount = new Mock<UserAccount>();
            var command = new LoginCommand { UserAccount = userAccount.Object, Cookies = endpoint.CookieStorage, HttpContext = endpoint.HttpContext };

            var redirect = endpoint.Login(command);
            
            userAccount.Verify(x => x.Login(endpoint.CookieStorage, endpoint.HttpContext));
            redirect.AssertWasRedirectedTo<HomeRequest>(x => x != null);
        }
    }

    internal class TestableLoginEndpoint : LoginEndpoint
    {
        public Mock<IUserAccountService> Service { get; private set; }
        public Mock<IMappingEngine> MappingEngine { get; private set; }
        public HttpContextBase HttpContext { get; private set; }
        public ICookieStorage CookieStorage { get; private set; }

        public TestableLoginEndpoint(Mock<IUserAccountService> service, Mock<IMappingEngine> mappingEngine, Mock<ICookieStorage> cookieStorage, HttpContextBase httpContext)
            : base(mappingEngine.Object)
        {
            Service = service;
            MappingEngine = mappingEngine;
            HttpContext = httpContext;
            CookieStorage = cookieStorage.Object;
        }

        public static TestableLoginEndpoint Build()
        {
            return new TestableLoginEndpoint(new Mock<IUserAccountService>(), new Mock<IMappingEngine>(), new Mock<ICookieStorage>(), new Mock<HttpContextBase>().Object);
        }
    }
}