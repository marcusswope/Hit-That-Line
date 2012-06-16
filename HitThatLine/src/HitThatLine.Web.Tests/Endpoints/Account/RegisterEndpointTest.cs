using AutoMapper;
using HitThatLine.Web.Endpoints.Account;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Services;
using Moq;
using NUnit.Framework;
using HitThatLine.Web.Tests.Utility;

namespace HitThatLine.Web.Tests.Endpoints.Account
{
    [TestFixture]
    public class RegisterEndpointTest
    {
        [Test]
        public void GET_MapRequestToModel()
        {
            var endpoint = new TestableRegisterEndpoint(new Mock<IUserAccountService>(), new Mock<IMappingEngine>());
            var request = new RegisterRequest();
            var expectedViewModel = new RegisterViewModel();

            endpoint.Mapper.Setup(x => x.Map<RegisterRequest, RegisterViewModel>(request)).Returns(expectedViewModel);
            
            var model = endpoint.Register(request);

            model.ShouldEqual(expectedViewModel);
        }

        [Test]
        public void POST_CreateNewAccountAndRedirect()
        {
            var endpoint = new TestableRegisterEndpoint(new Mock<IUserAccountService>(), new Mock<IMappingEngine>());
            var command = new RegisterCommand();

            var continuation = endpoint.Register(command);

            endpoint.Service.Verify(x => x.CreateNew(command));
            continuation.AssertWasRedirectedTo<HomeInputModel>();
        }
    }

    public class TestableRegisterEndpoint : RegisterEndpoint
    {
        public Mock<IUserAccountService> Service { get; private set; }
        public Mock<IMappingEngine> Mapper { get; private set; }

        public TestableRegisterEndpoint(Mock<IUserAccountService> service, Mock<IMappingEngine> mapper)
            :base(service.Object, mapper.Object)
        {
            Service = service;
            Mapper = mapper;
        }
    }
}