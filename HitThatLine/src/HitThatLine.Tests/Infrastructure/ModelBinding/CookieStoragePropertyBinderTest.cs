using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using HitThatLine.Infrastructure.ModelBinding;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class CookieStoragePropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToType_ICookieStorage()
        {
            var binder = new CookieStoragePropertyBinder();
            var model = new CookieRequestModel();

            binder.Matches(model.GetType().GetProperty("Cookies")).ShouldBeTrue();
            binder.Matches(model.GetType().GetProperty("NotCookies")).ShouldBeFalse();
        }

        [Test]
        public void SetsPropertyFromContextOnModel()
        {
            var binder = new CookieStoragePropertyBinder();
            var model = new CookieRequestModel();
            var context = new Mock<IBindingContext>();
            var cookies = new CookieStorage();

            context.Setup(x => x.Service<ICookieStorage>()).Returns(cookies);
            context.SetupGet(x => x.Object).Returns(model);

            binder.Bind(model.GetType().GetProperty("Cookies"), context.Object);

            model.Cookies.ShouldEqual(cookies);
        }

        private class CookieRequestModel
        {
            public ICookieStorage Cookies { get; set; }
            public string NotCookies { get; set; }
        }
    }
}