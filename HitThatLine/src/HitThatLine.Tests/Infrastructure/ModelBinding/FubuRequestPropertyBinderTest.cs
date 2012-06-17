using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using HitThatLine.Infrastructure.ModelBinding;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class FubuRequestPropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToType_IFubuRequest()
        {
            var binder = new FubuRequestPropertyBinder();
            var model = new FubuRequestModel();

            binder.Matches(model.GetType().GetProperty("Request")).ShouldBeTrue();
            binder.Matches(model.GetType().GetProperty("NotRequest")).ShouldBeFalse();
        }

        [Test]
        public void SetsPropertyFromContextOnModel()
        {
            var binder = new FubuRequestPropertyBinder();
            var model = new FubuRequestModel();
            var context = new Mock<IBindingContext>();
            var fubuRequest = new FubuRequest(null, null);

            context.Setup(x => x.Service<IFubuRequest>()).Returns(fubuRequest);
            context.SetupGet(x => x.Object).Returns(model);

            binder.Bind(model.GetType().GetProperty("Request"), context.Object);

            model.Request.ShouldEqual(fubuRequest);
        }

        private class FubuRequestModel
        {
            public IFubuRequest Request { get; set; }
            public string NotRequest { get; set; }
        }
    }
}