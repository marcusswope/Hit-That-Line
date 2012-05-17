using Raven.Client;
using Raven.Client.Document;
using StructureMap.Configuration.DSL;

namespace HitThatLine.Web.Infrastructure
{
    public class RavenDBRegistry : Registry
    {
        public RavenDBRegistry()
        {
            var store = new DocumentStore { ConnectionStringName = "RavenDB" };
            store.Conventions.IdentityPartsSeparator = "-";
            store.Initialize();

            For<IDocumentSession>().Use(store.OpenSession);
        }
    }
}