using AutoMapper;
using AutoMapper.Mappers;
using HitThatLine.Endpoints.Account.Models;
using StructureMap.Configuration.DSL;

namespace HitThatLine.Infrastructure
{
    public class AutoMapperRegistry : Registry
    {
        public AutoMapperRegistry()
        {
            var configStore = BuildConfigStore();

            ConfigureMaps(configStore);

            ConfigureStructureMap(configStore);
        }

        public static void ConfigureMaps(ConfigurationStore configStore)
        {
            configStore.CreateMap<SummaryRequest, SummaryViewModel>();
            configStore.CreateMap<LoginRequest, LoginViewModel>();
            configStore.CreateMap<LoginCommand, LoginRequest>();
            configStore.CreateMap<RegisterRequest, RegisterViewModel>();
            configStore.CreateMap<RegisterCommand, RegisterRequest>();
        }

        public void ConfigureStructureMap(ConfigurationStore configStore)
        {
            For<ConfigurationStore>().Singleton().Use(configStore);
            For<IConfigurationProvider>().Use(x => x.GetInstance<ConfigurationStore>());
            For<IConfiguration>().Use(x => x.GetInstance<ConfigurationStore>());
            For<IMappingEngine>().Use<MappingEngine>();
            For<ITypeMapFactory>().Use<TypeMapFactory>();
        }

        public static ConfigurationStore BuildConfigStore()
        {
            return new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers.Invoke());
        }
    }
}