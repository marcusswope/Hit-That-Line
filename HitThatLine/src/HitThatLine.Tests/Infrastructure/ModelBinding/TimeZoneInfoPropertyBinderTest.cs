using System;
using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.ModelBinding;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class TimeZoneInfoPropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToType_TimeSpan()
        {
            var cookies = new Mock<ICookieStorage>();
            var binder = new TimeZoneInfoPropertyBinder(cookies.Object);
            var model = new TimeZoneRequestModel();

            binder.Matches(model.GetType().GetProperty("TimeZoneInfo")).ShouldBeTrue();
            binder.Matches(model.GetType().GetProperty("NotTimeZoneInfo")).ShouldBeFalse();
        }

        [Test]
        public void SetsPropertyFromCookieValue()
        {
            var cookies = new Mock<ICookieStorage>();
            var binder = new TimeZoneInfoPropertyBinder(cookies.Object);
            var model = new TimeZoneRequestModel();
            var context = new Mock<IBindingContext>();

            cookies.Setup(x => x.Contains(UserAccount.TimeZoneCookieName)).Returns(true);
            cookies.Setup(x => x.Get(UserAccount.TimeZoneCookieName, false)).Returns("300");
            context.SetupGet(x => x.Object).Returns(model);

            binder.Bind(model.GetType().GetProperty("TimeZoneInfo"), context.Object);

            model.TimeZoneInfo.TotalMinutes.ShouldEqual(300);
        }

        private class TimeZoneRequestModel
        {
            public TimeSpan TimeZoneInfo { get; set; }
            public string NotTimeZoneInfo { get; set; }
        }
    }
}