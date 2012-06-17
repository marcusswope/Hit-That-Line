using System;
using System.Web.Hosting;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace HitThatLine.Infrastructure
{
    public abstract class BackgroundTask : IRegisteredObject
    {
        private readonly object _lock = new object();
        protected IDocumentSession Session;

        protected virtual void Initialize(IDocumentSession session)
        {
            Session = session;
            Session.Advanced.UseOptimisticConcurrency = true;
        }

        protected virtual void OnError(Exception e)
        {
        }

        public BackgroundTaskResult Run(IDocumentSession session)
        {
            Initialize(session);
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