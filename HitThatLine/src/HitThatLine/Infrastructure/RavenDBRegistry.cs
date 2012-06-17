using Raven.Client;
using Raven.Client.Document;
using StructureMap.Configuration.DSL;

namespace HitThatLine.Infrastructure
{
    public class RavenDBRegistry : Registry
    {
        public RavenDBRegistry()
        {
            var store = new DocumentStore { ConnectionStringName = "RavenDB", DefaultDatabase = "HitThatLine" };
            store.Conventions.IdentityPartsSeparator = "-";
            store.Initialize();
            TaskExecutor.DocumentStore = store;

            For<IDocumentSession>().Use(store.OpenSession);
        }
    }
}