using AutoMapper;
using HitThatLine.Domain.Accounts;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Home;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Infrastructure.AutoMapper;
using HitThatLine.Tests.Utility;
using NUnit.Framework;

namespace HitThatLine.Tests.Endpoints.Home
{
    [TestFixture]
    public class HomeEndpointTest : RavenTestBase
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void ReturnsTop20ThreadSummaries()
        {
            var configStore = AutoMapperRegistry.BuildConfigStore();
            AutoMapperRegistry.ConfigureMaps(configStore);

            var endpoint = new HomeEndpoint(Session, new MappingEngine(configStore));
            var thread = new DiscussionThread("title", "body", new[] {"tag"}, new UserAccount());
            Session.Store(thread);
            Session.SaveChanges();

            var viewModel = endpoint.Home(new HomeRequest());
            viewModel.Threads.ShouldContain(x => x.Title == thread.Title);
        }
    }
}