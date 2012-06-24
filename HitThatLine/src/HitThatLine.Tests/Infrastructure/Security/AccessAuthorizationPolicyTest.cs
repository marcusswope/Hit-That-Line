using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Thread;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Services;
using Moq;
using NUnit.Framework;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Infrastructure.Security
{
    [TestFixture]
    public class AccessAuthorizationPolicyTest
    {
        [Test]
        public void DeniesAccessIfNotLoggedIn()
        {
            var policy = TestableAccessAuthorizationPolicy.Build();

            policy.Service.Setup(x => x.GetCurrent()).Returns(null as UserAccount);

            policy.RightsFor(policy.Request.Object).ShouldEqual(AuthorizationRight.Deny);
        }

        [Test]
        public void DeniesAccessIfRolesAreIncorrect()
        {
            var policy = TestableAccessAuthorizationPolicy.Build();

            policy.Service.Setup(x => x.GetCurrent()).Returns(new UserAccount());

            policy.RightsFor(policy.Request.Object).ShouldEqual(AuthorizationRight.Deny);
        }

        [Test]
        public void AllowsAccessIfRolesAreCorrect()
        {
            var policy = TestableAccessAuthorizationPolicy.Build();
            var userAccount = new UserAccount();
            userAccount.Roles.Add(UserAccount.BasicUserRole);

            policy.Service.Setup(x => x.GetCurrent()).Returns(userAccount);

            policy.RightsFor(policy.Request.Object).ShouldEqual(AuthorizationRight.Allow);
        }

        public class TestableAccessAuthorizationPolicy : AccessAuthorizationPolicy<NewThreadEndpoint>
        {
            public Mock<IUserAccountService> Service { get; set; }
            public Mock<IFubuRequest> Request { get; set; }

            public TestableAccessAuthorizationPolicy(Mock<IUserAccountService> service, Mock<IFubuRequest> request)
                : base(service.Object)
            {
                Service = service;
                Request = request;
            }

            public static TestableAccessAuthorizationPolicy Build()
            {
                return new TestableAccessAuthorizationPolicy(new Mock<IUserAccountService>(), new Mock<IFubuRequest>());
            }
        }
    }
}