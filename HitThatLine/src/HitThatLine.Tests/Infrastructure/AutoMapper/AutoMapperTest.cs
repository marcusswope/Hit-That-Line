using HitThatLine.Infrastructure.AutoMapper;
using NUnit.Framework;

namespace HitThatLine.Tests.Infrastructure.AutoMapper
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