using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.AutoMapper;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure
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