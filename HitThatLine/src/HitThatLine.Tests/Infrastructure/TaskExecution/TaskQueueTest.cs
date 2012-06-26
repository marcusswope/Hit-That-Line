using System.Threading;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.TaskExecution;
using HitThatLine.Tests.Utility;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.TaskExecution
{
    [TestFixture]
    public class TaskQueueTest
    {
        [TestFixture]
        public class Integration : RavenTestBase
        {
            [Test]
            public void ExecutesTestsAsync()
            {
                Session.Dispose();
                var queue = new TaskQueue(DocumentStore);

                var task1 = new LongRunningUpdateName(DefaultUser.Username, "1", 750);
                queue.ExecuteLater(task1);
                queue.StartExecuting();
                queue.Discard();

                var task2 = new LongRunningUpdateName(DefaultUser.Username, "2", 0);
                queue.ExecuteLater(task2);
                queue.StartExecuting();
                queue.Discard();

                task1.Stop(true);
                task2.Stop(true);
                Thread.Sleep(2000);
                Session = DocumentStore.OpenSession();

                var account = Session.Load<UserAccount>(DefaultUser.DocumentKey);
                account.Username.ShouldEqual("user21");
            }

            private class LongRunningUpdateName : BackgroundTask
            {
                private readonly string _username;
                private readonly string _appendValue;
                private readonly int _timeoutMillis;

                public LongRunningUpdateName(string username, string appendValue, int timeoutMillis)
                {
                    _username = username;
                    _appendValue = appendValue;
                    _timeoutMillis = timeoutMillis;
                }

                public override void Execute()
                {
                    Thread.Sleep(_timeoutMillis);
                    var account = Session.Load<UserAccount>(UserAccount.Key(_username));
                    account.Username = account.Username + _appendValue;
                }
            }
        }
    }
}