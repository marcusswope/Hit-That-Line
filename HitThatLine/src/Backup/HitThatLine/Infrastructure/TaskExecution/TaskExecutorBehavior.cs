using FubuMVC.Core.Behaviors;

namespace HitThatLine.Infrastructure.TaskExecution
{
    public class TaskExecutorBehavior : IActionBehavior
    {
        private readonly ITaskQueue _taskQueue;
        public TaskExecutorBehavior(ITaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        public IActionBehavior InsideBehavior { get; set; }

        public void Invoke()
        {
            InsideBehavior.Invoke();
            _taskQueue.StartExecuting();
        }

        public void InvokePartial()
        {
            InsideBehavior.InvokePartial();
            _taskQueue.StartExecuting();
        }
    }
}