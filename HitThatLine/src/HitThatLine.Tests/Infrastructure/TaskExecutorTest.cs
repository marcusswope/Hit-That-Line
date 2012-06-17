using System.Threading;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure;
using HitThatLine.Tests.Utility;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure
{
    [TestFixture]
    public class TaskExecutorTest
    {
        [TestFixture]
        public class Integration : RavenTestBase
        {
            [Test]
            public void ExecutesTestsAsync()
            {
                Session.Dispose();
                TaskExecutor.DocumentStore = DocumentStore;

                var task1 = new LongRunningUpdateName(DefaultUser.Id, "1", 1000);
                TaskExecutor.ExcuteLater(task1);
                TaskExecutor.StartExecuting();
                TaskExecutor.Discard();

                var task2 = new LongRunningUpdateName(DefaultUser.Id, "2", 500);
                TaskExecutor.ExcuteLater(task2);
                TaskExecutor.StartExecuting();
                TaskExecutor.Discard();

                task1.Stop(true);
                task2.Stop(true);
                Thread.Sleep(5000);
                Session = DocumentStore.OpenSession();
                
                var account = Session.Load<UserAccount>(DefaultUser.Id);
                account.Username.ShouldEqual("user21");
            }

            private class LongRunningUpdateName : BackgroundTask
            {
                private readonly string _id;
                private readonly string _appendValue;
                private readonly int _timeoutMillis;

                public LongRunningUpdateName(string id, string appendValue, int timeoutMillis)
                {
                    _id = id;
                    _appendValue = appendValue;
                    _timeoutMillis = timeoutMillis;
                }

                public override void Execute()
                {
                    var account = Session.Load<UserAccount>(_id);
                    Thread.Sleep(_timeoutMillis);
                    account.Username = account.Username + _appendValue;
                }
            }
        }
    }
}