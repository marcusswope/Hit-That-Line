using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using Raven.Client;

namespace HitThatLine.Infrastructure
{
    public static class TaskExecutor
    {
        private static readonly ThreadLocal<List<BackgroundTask>> tasksToExecute =
            new ThreadLocal<List<BackgroundTask>>(() => new List<BackgroundTask>());

        public static IDocumentStore DocumentStore
        {
            get { return _documentStore; }
            set
            {
                if (_documentStore == null)
                {
                    _documentStore = value;
                }
            }
        }
        private static IDocumentStore _documentStore;

        public static Action<Exception> ExceptionHandler { get; set; }

        public static void ExcuteLater(BackgroundTask task)
        {
            tasksToExecute.Value.Add(task);
        }

        public static void Discard()
        {
            tasksToExecute.Value.Clear();
        }

        public static void StartExecuting()
        {
            var value = tasksToExecute.Value;
            var copy = value.ToArray();
            value.Clear();

            if (copy.Length > 0)
            {
                Task.Factory.StartNew(() =>
                                        {
                                            foreach (var backgroundTask in copy)
                                            {
                                                HostingEnvironment.RegisterObject(backgroundTask);
                                                var success = ExecuteTask(backgroundTask);
                                                HostingEnvironment.UnregisterObject(backgroundTask);
                                                if (!success) return;
                                            }
                                        }, TaskCreationOptions.LongRunning)
                            .ContinueWith(task =>
                                        {
                                            if (ExceptionHandler != null)
                                            {
                                                ExceptionHandler(task.Exception);
                                            }
                                        }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        public static bool ExecuteTask(BackgroundTask task)
        {
            for (var i = 0; i < 10; i++)
            {
                using (var session = _documentStore.OpenSession())
                {
                    var result = task.Run(session);
                    switch (result)
                    {
                        case BackgroundTaskResult.Continue:
                            return true;
                        case BackgroundTaskResult.Stop:
                            return false;
                        case BackgroundTaskResult.TryAgain:
                            break;
                    }
                }
            }
            return false;
        }
    }
}