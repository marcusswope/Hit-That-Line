using FubuCore.Binding;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.ModelBinding;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;
using Raven.Client;

namespace HitThatLine.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class UserAccountPropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToType_UserAccount()
        {
            var binder = new UserAccountPropertyBinder();
            binder.Matches(typeof(UserAccountBindingModel).GetProperty("UserAccount")).ShouldBeTrue();
            binder.Matches(typeof(UserAccountBindingModel).GetProperty("NotAUserAccount")).ShouldBeFalse();
        }

        [Test]
        public void GetsFromServiceAndSets()
        {
            var binder = TestableUserAccountPropertyBinder.Build();
            var account = new UserAccount();
            binder.Service.Setup(x => x.GetCurrent()).Returns(account);

            binder.Bind(typeof(UserAccountBindingModel).GetProperty("UserAccount"), binder.Context.Object);

            binder.Model.UserAccount.ShouldEqual(account);
        }

        private class TestableUserAccountPropertyBinder : UserAccountPropertyBinder
        {
            public Mock<IUserAccountService> Service { get; private set; }
            public Mock<IBindingContext> Context { get; private set; }
            public UserAccountBindingModel Model { get; private set; }
            
            public TestableUserAccountPropertyBinder()
            {
                Service = new Mock<IUserAccountService>();
                Context = new Mock<IBindingContext>();
                Model = new UserAccountBindingModel();

                Context.SetupGet(x => x.Object).Returns(Model);
                Context.Setup(x => x.Service<IUserAccountService>()).Returns(Service.Object);
            }

            public static TestableUserAccountPropertyBinder Build()
            {
                return new TestableUserAccountPropertyBinder();
            }
        }

        private class UserAccountBindingModel
        {
            public UserAccount UserAccount { get; set; }
            public string NotAUserAccount { get; set; }
            public string Username { get; set; }
        }
    }
}