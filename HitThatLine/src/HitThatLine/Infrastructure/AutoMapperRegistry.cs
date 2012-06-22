using AutoMapper;
using AutoMapper.Mappers;
using HitThatLine.Domain.Discussion;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Thread.Models;
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

        public static void ConfigureMaps(IConfiguration configStore)
        {
            configStore.CreateMap<SummaryRequest, SummaryViewModel>();
            configStore.CreateMap<LoginRequest, LoginViewModel>();
            configStore.CreateMap<RegisterRequest, RegisterViewModel>();
            configStore.CreateMap<NewThreadRequest, NewThreadViewModel>();
            configStore.CreateMap<DiscussionThread, ViewThreadRequest>();
            
            configStore.CreateMap<LoginCommand, LoginRequest>()
                .ForMember(x => x.Password, opt => opt.Ignore());

            configStore.CreateMap<RegisterCommand, RegisterRequest>()
                .ForMember(x => x.Password, opt => opt.Ignore())
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore());
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