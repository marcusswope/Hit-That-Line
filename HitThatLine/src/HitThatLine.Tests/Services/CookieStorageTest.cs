using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using HitThatLine.Services;
using Moq;
using NUnit.Framework;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Services
{
    [TestFixture]
    public class CookieStorageTest
    {
        [TestFixture]
        public class Contains
        {
            [Test]
            public void FalseIfNull()
            {
                var storage = TestableCookieStorage.Build();
                storage.Contains("test").ShouldBeFalse();
            }

            [Test]
            public void TrueIfNotNull()
            {
                var storage = TestableCookieStorage.Build();
                storage.RequestCookies.Add(new HttpCookie("test"));
                storage.Contains("test").ShouldBeTrue();
            }
        }

        [TestFixture]
        public class Set
        {
            [Test]
            public void EncryptsAndStoresCookieFor20YearsOnResponse()
            {
                var storage = TestableCookieStorage.Build();
                storage.Set("test", "testValue");

                var responseCookie = storage.ResponseCookies["test"];
                responseCookie.Name.ShouldEqual("test");
                responseCookie.Expires.ShouldBeWithinOneSecondFrom(DateTime.Now.AddYears(20));

                var encryptedBytes1 = Convert.FromBase64String(responseCookie.Value);
                var decryptedBytes = ProtectedData.Unprotect(encryptedBytes1, null, DataProtectionScope.CurrentUser);
                Encoding.UTF8.GetString(decryptedBytes).ShouldEqual("testValue");
            }
        }

        [TestFixture]
        public class Get
        {
            [Test]
            public void DecryptsFromRequestAndReturnsValue()
            {
                var storage = TestableCookieStorage.Build();
                storage.Set("test", "testValue");
                storage.RequestCookies.Add(storage.ResponseCookies["test"]);

                storage.Get("test").ShouldEqual("testValue");
            }
        }

        [TestFixture]
        public class Remove
        {
            [Test]
            public void SetsCookieOnResponseWithPastExpirationDate()
            {
                var storage = TestableCookieStorage.Build();
                storage.Remove("test");
                storage.ResponseCookies["test"].Expires.ShouldBeLessThan(DateTime.Now);
            }
        }


        private class TestableCookieStorage : CookieStorage
        {
            public Mock<HttpContextBase> HttpContext { get; private set; }
            public Mock<HttpRequestBase> HttpRequest { get; private set; }
            public Mock<HttpResponseBase> HttpResponse { get; private set; }
            public HttpCookieCollection RequestCookies { get; private set; }
            public HttpCookieCollection ResponseCookies { get; private set; }

            public TestableCookieStorage(Mock<HttpContextBase> httpContext, Mock<HttpRequestBase> httpRequest, HttpCookieCollection cookies, Mock<HttpResponseBase> httpResponse, HttpCookieCollection responseCookies)
                : base(httpContext.Object)
            {
                HttpContext = httpContext;
                HttpRequest = httpRequest;
                RequestCookies = cookies;
                HttpResponse = httpResponse;
                ResponseCookies = responseCookies;

                HttpContext.SetupGet(x => x.Request).Returns(HttpRequest.Object);
                HttpContext.SetupGet(x => x.Response).Returns(HttpResponse.Object);
                
                HttpRequest.SetupGet(x => x.Cookies).Returns(RequestCookies);
                HttpResponse.SetupGet(x => x.Cookies).Returns(ResponseCookies);
            }

            public static TestableCookieStorage Build()
            {
                return new TestableCookieStorage(new Mock<HttpContextBase>(), new Mock<HttpRequestBase>(), new HttpCookieCollection(), new Mock<HttpResponseBase>(), new HttpCookieCollection());
            }
        }
    }
}