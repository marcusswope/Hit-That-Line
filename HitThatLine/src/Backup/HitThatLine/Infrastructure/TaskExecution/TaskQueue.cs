using Raven.Client;

namespace HitThatLine.Infrastructure.TaskExecution
{
    public interface ITaskQueue
    {
        void ExecuteLater(BackgroundTask task);
        void Discard();
        void StartExecuting();
    }

    public class TaskQueue : ITaskQueue
    {
        public TaskQueue(IDocumentStore store)
        {
            TaskExecutor.DocumentStore = store;
        }

        public void ExecuteLater(BackgroundTask task)
        {
            TaskExecutor.ExecuteLater(task);
        }

        public void Discard()
        {
            TaskExecutor.Discard();
        }

        public void StartExecuting()
        {
            TaskExecutor.StartExecuting();
        }
    }
}