using HitThatLine.Web.App_Start;
using NUnit.Framework;

namespace HitThatLine.Web.Tests.Infrastructure
{
    [TestFixture]
    public class AppStartFubuMVCTest
    {
        [Test]
        public void AssertAppStartWithoutErrors()
        {
            AppStartFubuMVC.Start();
        } 
    }
}