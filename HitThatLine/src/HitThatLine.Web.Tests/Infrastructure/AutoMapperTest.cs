using HitThatLine.Web.Infrastructure;
using NUnit.Framework;

namespace HitThatLine.Web.Tests.Infrastructure
{
    [TestFixture]
    public class AutoMapperTest
    {
        [Test]
        public void ConfigurationIsValid()
        {
            var configStore = AutoMapperRegistry.BuildConfigStore();
            AutoMapperRegistry.ConfigureMaps(configStore);

            configStore.AssertConfigurationIsValid();
        }
    }
}