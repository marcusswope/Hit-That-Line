using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.MvcIntegration;
using StructureMap.Configuration.DSL;

namespace HitThatLine.Infrastructure.Raven
{
    public class RavenDBRegistry : Registry
    {
        public RavenDBRegistry()
        {
            var store = new DocumentStore { ConnectionStringName = "RavenDB", DefaultDatabase = "HitThatLine" };
            store.Conventions.IdentityPartsSeparator = "-";
            store.Initialize();
            RavenProfiler.InitializeFor(store);
            IndexCreation.CreateIndexes(typeof(Threads_TagCount).Assembly, store);
            TaskExecutor.DocumentStore = store;

            For<IDocumentSession>().Use(store.OpenSession);
        }
    }
}