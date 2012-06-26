using System.Collections.Specialized;
using System.Web;
using FubuCore.Binding;
using HitThatLine.Infrastructure.ModelBinding;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class IPAddressPropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToStringsNamedIPAddress()
        {
            var binder = new IPAddressPropertyBinder();
            var model = new IPAddressRequestModel();

            binder.Matches(model.GetType().GetProperty("IPAddress")).ShouldBeTrue();
            binder.Matches(model.GetType().GetProperty("NotAnIPAddress")).ShouldBeFalse();
        }

        [Test]
        public void TriesToSetFromProxyPassthrough1()
        {
            var ipAddress = "192.168.1.1";
            var binder = new IPAddressPropertyBinder();
            var model = new IPAddressRequestModel();
            var context = setupContext("HTTP_X_FORWARDED_FOR", ipAddress, model);

            binder.Bind(model.GetType().GetProperty("IPAddress"), context.Object);

            model.IPAddress.ShouldEqual(ipAddress);
        }

        [Test]
        public void TriesToSetFromProxyPassthrough2()
        {
            var ipAddress = "192.168.1.1";
            var binder = new IPAddressPropertyBinder();
            var model = new IPAddressRequestModel();
            var context = setupContext("HTTP_X_FORWARDED", ipAddress, model);

            binder.Bind(model.GetType().GetProperty("IPAddress"), context.Object);

            model.IPAddress.ShouldEqual(ipAddress);
        }

        [Test]
        public void SetsFromUserHostAddress()
        {
            var binder = new IPAddressPropertyBinder();
            var model = new IPAddressRequestModel();
            var context = setupContext("HTTP_X_FORWARDED", null, model);

            binder.Bind(model.GetType().GetProperty("IPAddress"), context.Object);

            model.IPAddress.ShouldEqual("userHostAddress");
        }

        private static Mock<IBindingContext> setupContext(string headerName, string ipAddress, IPAddressRequestModel model)
        {
            var context = new Mock<IBindingContext>();
            var httpContext = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();

            var serverVariables = new NameValueCollection {{headerName, ipAddress}};

            context.Setup(x => x.Object).Returns(model);
            context.Setup(x => x.Service<HttpContextBase>()).Returns(httpContext.Object);
            httpContext.SetupGet(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(serverVariables);
            request.Setup(x => x.UserHostAddress).Returns("userHostAddress");
            return context;
        }

        public class IPAddressRequestModel
        {
            public string IPAddress { get; set; }
            public string NotAnIPAddress { get; set; }
        }
    }
}