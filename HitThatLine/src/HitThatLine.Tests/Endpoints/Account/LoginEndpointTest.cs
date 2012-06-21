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
using Raven.Client;

namespace HitThatLine.Tests.Endpoints.Account
{
    [TestFixture]
    public class LoginEndpointTest
    {
        [TestFixture]
        public class Get : RavenTestBase
        {
            [Test]
            public void MapsRequestToViewModel()
            {
                var endpoint = TestableLoginEndpoint.Build(Session);
                var request = new LoginRequest();
                var expectedViewModel = new LoginViewModel();

                endpoint.MappingEngine.Setup(x => x.Map<LoginRequest, LoginViewModel>(request)).Returns(expectedViewModel);
                var viewModel = endpoint.Login(request);
                viewModel.ShouldEqual(expectedViewModel);
            }
        }

        [TestFixture]
        public class Post : RavenTestBase
        {
            [Test]
            public void LogsInAndRedirects()
            {
                var endpoint = TestableLoginEndpoint.Build(Session);
                var command = new LoginCommand { Username = DefaultUser.Username, Cookies = endpoint.CookieStorage.Object, HttpContext = endpoint.HttpContext };

                var redirect = endpoint.Login(command);

                endpoint.CookieStorage.Verify(x => x.Set(UserAccount.LoginCookieName, DefaultUser.DocumentKey));
                redirect.AssertWasRedirectedTo<HomeRequest>(x => x != null);
            }
        }
    }

    internal class TestableLoginEndpoint : LoginEndpoint
    {
        public Mock<IUserAccountService> Service { get; private set; }
        public Mock<IMappingEngine> MappingEngine { get; private set; }
        public HttpContextBase HttpContext { get; private set; }
        public Mock<ICookieStorage> CookieStorage { get; private set; }

        public TestableLoginEndpoint(Mock<IUserAccountService> service, Mock<IMappingEngine> mappingEngine, Mock<ICookieStorage> cookieStorage, HttpContextBase httpContext, IDocumentSession session)
            : base(mappingEngine.Object, session)
        {
            Service = service;
            MappingEngine = mappingEngine;
            HttpContext = httpContext;
            CookieStorage = cookieStorage;
        }

        public static TestableLoginEndpoint Build(IDocumentSession session)
        {
            return new TestableLoginEndpoint(new Mock<IUserAccountService>(), new Mock<IMappingEngine>(), new Mock<ICookieStorage>(), new Mock<HttpContextBase>().Object, session);
        }
    }
}