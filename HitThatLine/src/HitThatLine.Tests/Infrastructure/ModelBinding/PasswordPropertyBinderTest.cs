using System.Web.Security;
using FubuCore.Binding;
using HitThatLine.Infrastructure.ModelBinding;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class PasswordPropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToPasswordWhenModelAlsoHasAUsername()
        {
            var binder = new PasswordPropertyBinder();
            binder.Matches(typeof(PasswordBindingModel).GetProperty("Password")).ShouldBeTrue();
            binder.Matches(typeof(PasswordBindingModel).GetProperty("Username")).ShouldBeFalse();

            binder.Matches(typeof(NotAPasswordBindingModel).GetProperty("Password")).ShouldBeFalse();
        }

        [Test]
        public void SaltsPasswordWithUsernameAndStoresToPasswordProperty()
        {
            var binder = new PasswordPropertyBinder();
            var model = new PasswordBindingModel {Username = "username", Password = "password12"};
            var expectedHashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password + model.Username, "SHA1");

            var context = new Mock<IBindingContext>();
            var contextValues = new Mock<IContextValues>();
            context.SetupGet(x => x.Object).Returns(model);
            context.SetupGet(x => x.Data).Returns(contextValues.Object);
            contextValues.Setup(x => x.ValueAs<string>("Username")).Returns(model.Username);
            contextValues.Setup(x => x.ValueAs<string>("Password")).Returns(model.Password);

            binder.Bind(typeof(PasswordBindingModel).GetProperty("Password"), context.Object);

            model.Password.ShouldEqual(expectedHashedPassword);
        }

        private class PasswordBindingModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class NotAPasswordBindingModel
        {
            public string Password { get; set; }
        }
    }
}