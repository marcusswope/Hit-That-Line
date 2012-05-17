using FubuMVC.Core.Behaviors;
using Raven.Client;

namespace HitThatLine.Web.Infrastructure.Behaviors
{
    public class RavenBehavior : IActionBehavior
    {
        private readonly IDocumentSession _session;
        public RavenBehavior(IDocumentSession session)
        {
            _session = session;
        }

        public IActionBehavior InsideBehavior { get; set; }

        public void Invoke()
        {
            InsideBehavior.Invoke();
            _session.SaveChanges();
        }

        public void InvokePartial()
        {
            InsideBehavior.InvokePartial();
        }
    }
}