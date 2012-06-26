using System;
using System.Web.Hosting;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace HitThatLine.Infrastructure.TaskExecution
{
    public abstract class BackgroundTask : IRegisteredObject
    {
        private readonly object _lock = new object();
        protected IDocumentSession Session;
        protected IDocumentStore Store;

        public virtual void Initialize(IDocumentSession session, IDocumentStore store)
        {
            Session = session;
            Store = store;
            Session.Advanced.UseOptimisticConcurrency = true;
            HostingEnvironment.RegisterObject(this);
        }

        protected virtual void OnError(Exception e)
        {
        }

        public BackgroundTaskResult Run(IDocumentSession session, IDocumentStore store)
        {
            Initialize(session, store);
            try
            {
                Execute();
                Session.SaveChanges();
                TaskExecutor.StartExecuting();
                return BackgroundTaskResult.Continue;
            }
            catch (ConcurrencyException e)
            {
                OnError(e);
                return BackgroundTaskResult.TryAgain;
            }
            catch (Exception e)
            {
                OnError(e);
                return BackgroundTaskResult.Stop;
            }
            finally
            {
                HostingEnvironment.UnregisterObject(this);
                TaskExecutor.Discard();
            }
        }

        public abstract void Execute();
        
        public void Stop(bool immediate)
        {
            if (immediate)
            {
                lock (_lock)
                {
                    HostingEnvironment.UnregisterObject(this);
                }
            }
        }
    }

    public enum BackgroundTaskResult
    {
        Continue,
        Stop,
        TryAgain
    }
}