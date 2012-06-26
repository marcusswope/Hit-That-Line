using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Thread.Models;
using HitThatLine.Endpoints.Thread.Tasks;
using HitThatLine.Tests.Utility;
using NUnit.Framework;
using Raven.Client.Linq;
using System.Linq;

namespace HitThatLine.Tests.Endpoints.Thread.Tasks
{
    [TestFixture]
    public class RecordThreadViewTaskTest : RavenTestBase
    {
        [Test]
        public void RecordsThreadViewIfNotViewedFromUserAndIPBefore()
        {
            var thread = new DiscussionThread("title", "body", new[] {"tags"}, DefaultUser);
            thread.ViewCount.ShouldEqual(0);
            Session.Store(thread);
            Session.SaveChanges();

            var request = new ViewThreadRequest
                              {
                                  IPAddress = "myIP", 
                                  UriId = thread.UriId,
                                  UserAccount = DefaultUser
                              };
            var task = new RecordThreadViewTask(request);
            task.Initialize(Session, DocumentStore);
            task.Execute();
            Session.SaveChanges();

            var views = Session.Query<ThreadView>().Where(x => x.IPAddress == request.IPAddress && x.Username == DefaultUser.Username && x.DiscussionThreadId == thread.Id).ToList();
            views.Count().ShouldEqual(1);
            Session.Load<DiscussionThread>(thread.Id).ViewCount.ShouldEqual(1);
        }
    }
}