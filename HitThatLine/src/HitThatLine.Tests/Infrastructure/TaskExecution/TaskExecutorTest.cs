using System.Threading;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.TaskExecution;
using HitThatLine.Tests.Utility;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.TaskExecution
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

                var task1 = new LongRunningUpdateName(DefaultUser.Username, "1", 500);
                TaskExecutor.ExcuteLater(task1);
                TaskExecutor.StartExecuting();
                TaskExecutor.Discard();

                var task2 = new LongRunningUpdateName(DefaultUser.Username, "2", 0);
                TaskExecutor.ExcuteLater(task2);
                TaskExecutor.StartExecuting();
                TaskExecutor.Discard();

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
                    var account = Session.Load<UserAccount>(UserAccount.Key(_username));
                    Thread.Sleep(_timeoutMillis);
                    account.Username = account.Username + _appendValue;
                }
            }
        }
    }
}