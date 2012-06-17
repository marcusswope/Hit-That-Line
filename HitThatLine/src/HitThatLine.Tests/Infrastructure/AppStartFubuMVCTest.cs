using HitThatLine.App_Start;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure
{
    [TestFixture]
    public class AppStartFubuMVCTest
    {
        [Test]
        public void AssertAppStartWithoutErrors()
        {
            new Global().Application_Start();
            AppStartFubuMVC.Start();
        } 
    }
}