using HitThatLine.Core.Accounts;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace HitThatLine.Web.Tests.Utility
{
    public abstract class RavenTestBase
    {
        protected IDocumentSession Session;
        protected UserAccount DefaultUser;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            var store = new EmbeddableDocumentStore { RunInMemory = true, UseEmbeddedHttpServer = true }.Initialize();
            Session = store.OpenSession();
            DefaultUser = new UserAccount { Username = "user", Password = "password", EmailAddress = "email@email.com" };
            Session.Store(DefaultUser);
            Session.SaveChanges();
        }
    }
}