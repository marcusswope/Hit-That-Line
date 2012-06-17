using System.Web;
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
    public class HttpContextPropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToType_HttpContextBase()
        {
            var binder = new HttpContextPropertyBinder();
            var model = new HttpContextRequestModel();

            binder.Matches(model.GetType().GetProperty("HttpContext")).ShouldBeTrue();
            binder.Matches(model.GetType().GetProperty("NotHttpContext")).ShouldBeFalse();
        }

        [Test]
        public void SetsPropertyFromContextOnModel()
        {
            var binder = new HttpContextPropertyBinder();
            var model = new HttpContextRequestModel();
            var context = new Mock<IBindingContext>();
            var httpContext = new Mock<HttpContextBase>().Object;

            context.Setup(x => x.Service<HttpContextBase>()).Returns(httpContext);
            context.SetupGet(x => x.Object).Returns(model);

            binder.Bind(model.GetType().GetProperty("HttpContext"), context.Object);

            model.HttpContext.ShouldEqual(httpContext);
        }

        private class HttpContextRequestModel
        {
            public HttpContextBase HttpContext { get; set; }
            public string NotHttpContext { get; set; }
        }
    }
}