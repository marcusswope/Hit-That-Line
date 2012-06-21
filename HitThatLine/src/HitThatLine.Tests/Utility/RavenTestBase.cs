using HitThatLine.Domain.Accounts;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace HitThatLine.Tests.Utility
{
    public abstract class RavenTestBase
    {
        protected IDocumentSession Session;
        protected IDocumentStore DocumentStore;
        protected UserAccount DefaultUser;
        
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            DocumentStore = new EmbeddableDocumentStore { RunInMemory = true, UseEmbeddedHttpServer = true }.Initialize();
            Session = DocumentStore.OpenSession();
            DefaultUser = new UserAccount { Username = "user", Password = "password", EmailAddress = "email@email.com" };
            
            Session.Store(DefaultUser, DefaultUser.DocumentKey);
            Session.SaveChanges();
        }
    }
}