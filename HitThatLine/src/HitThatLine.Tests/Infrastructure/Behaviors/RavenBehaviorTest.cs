using FubuMVC.Core.Behaviors;
using HitThatLine.Infrastructure.Behaviors;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;
using Raven.Client;

namespace HitThatLine.Tests.Infrastructure.Behaviors
{
    [TestFixture]
    public class RavenBehaviorTest
    {
        [Test]
        public void CallsSaveChangesAfterInsideBehavior()
        {
            var session = new Mock<IDocumentSession>();
            var insideBehavior = new MockActionBehavior();
            insideBehavior.OnInvoke(() => session.Verify(x => x.SaveChanges(), Times.Never()));

            var behavior = new RavenBehavior(session.Object);
            behavior.InsideBehavior = insideBehavior;

            behavior.Invoke();

            session.Verify(x => x.SaveChanges());
            insideBehavior.VerifyInvoked();
        }

        [Test]
        public void DoesntCallSaveChangesForPartial()
        {
            var session = new Mock<IDocumentSession>();
            var insideBehavior = new Mock<IActionBehavior>();

            var behavior = new RavenBehavior(session.Object);
            behavior.InsideBehavior = insideBehavior.Object;

            behavior.InvokePartial();

            session.Verify(x => x.SaveChanges(), Times.Never());
            insideBehavior.Verify(x => x.InvokePartial());
        }
    }
}