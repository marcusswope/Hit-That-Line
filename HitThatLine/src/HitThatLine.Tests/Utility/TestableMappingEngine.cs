using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.AutoMapper;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Utility
{
    public class TestableMappingEngine : IMappingEngine
    {
        public TestableConfigurationStore ConfigurationProvider { get; private set; }
        public Mock<IMappingEngine> Mapper { get; private set; }

        public TestableMappingEngine()
        {
            ConfigurationProvider = new TestableConfigurationStore();
            AutoMapperRegistry.ConfigureMaps(ConfigurationProvider);
            Mapper = new Mock<IMappingEngine>();
        }

        public void Dispose()
        {
            Mapper.Object.Dispose();
        }

        public TDestination Map<TDestination>(object source)
        {
            mapMustExist(source.GetType(), typeof(TDestination));
            return Mapper.Object.Map<TDestination>(source);
        }

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            mapMustExist(typeof(TSource), typeof(TDestination));
            return Mapper.Object.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            mapMustExist(typeof(TSource), typeof(TDestination));
            return Mapper.Object.Map(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            mapMustExist(sourceType, destinationType);
            return Mapper.Object.Map(source, destination, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        private void mapMustExist(Type source, Type destination)
        {
            if (!ConfigurationProvider.HasMapFor(source, destination))
            {
                throw new AutoMapperConfigurationException(string.Format("No map configured Source: {0} Destination {1}", source.Name, destination.Name));
            }
        }

        public TDestination DynamicMap<TSource, TDestination>(TSource source)
        {
            throw new NotImplementedException();
        }

        public TDestination DynamicMap<TDestination>(object source)
        {
            throw new NotImplementedException();
        }

        public object DynamicMap(object source, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException();
        }

        public void DynamicMap(object source, object destination, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public Expression<Func<TSource, TDestination>> CreateMapExpression<TSource, TDestination>()
        {
            throw new NotImplementedException();
        }
    }

    public class TestableConfigurationStore : IConfigurationProvider, IConfiguration
    {
        private readonly Dictionary<Type, Type> _maps;
        public TestableConfigurationStore()
        {
            _maps = new Dictionary<Type, Type>();
        }

        public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return CreateMap<TSource, TDestination>(MemberList.Source);
        }

        public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(MemberList source)
        {
            createMap(typeof(TSource), typeof(TDestination));
            return new TestableMappingExpression<TSource, TDestination>();
        }

        public IMappingExpression CreateMap(Type sourceType, Type destinationType)
        {
            return CreateMap(sourceType, destinationType, MemberList.Source);
        }

        public IMappingExpression CreateMap(Type sourceType, Type destinationType, MemberList source)
        {
            createMap(sourceType, destinationType);
            return new TestableMappingExpression();
        }

        private void createMap(Type sourceType, Type destinationType)
        {
            if (_maps.ContainsKey(sourceType) && _maps[sourceType] == destinationType)
            {
                throw new AssertionException("Two maps configured for the same source and destination types: {0} and {1}");
            }

            _maps.Add(sourceType, destinationType);
        }

        public bool MapNullSourceValuesAsNull
        {
            get { throw new NotImplementedException(); }
        }

        public bool MapNullSourceCollectionsAsNull
        {
            get { throw new NotImplementedException(); }
        }

        public TypeMap[] GetAllTypeMaps()
        {
            throw new NotImplementedException();
        }

        public TypeMap FindTypeMapFor(object source, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public TypeMap FindTypeMapFor(Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public TypeMap FindTypeMapFor(ResolutionResult resolutionResult, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public IFormatterConfiguration GetProfileConfiguration(string profileName)
        {
            throw new NotImplementedException();
        }

        public void AssertConfigurationIsValid()
        {
            throw new NotImplementedException();
        }

        public void AssertConfigurationIsValid(TypeMap typeMap)
        {
            throw new NotImplementedException();
        }

        public void AssertConfigurationIsValid(string profileName)
        {
            throw new NotImplementedException();
        }

        public IObjectMapper[] GetMappers()
        {
            throw new NotImplementedException();
        }

        public TypeMap CreateTypeMap(Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public Func<Type, object> ServiceCtor
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<TypeMapCreatedEventArgs> TypeMapCreated;
        public IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
        {
            throw new NotImplementedException();
        }

        public IFormatterCtorExpression AddFormatter(Type valueFormatterType)
        {
            throw new NotImplementedException();
        }

        public void AddFormatter(IValueFormatter formatter)
        {
            throw new NotImplementedException();
        }

        public void AddFormatExpression(Func<ResolutionContext, string> formatExpression)
        {
            throw new NotImplementedException();
        }

        public void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
        {
            throw new NotImplementedException();
        }

        public IFormatterExpression ForSourceType<TSource>()
        {
            throw new NotImplementedException();
        }

        public bool AllowNullDestinationValues
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool AllowNullCollections
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public INamingConvention SourceMemberNamingConvention
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public INamingConvention DestinationMemberNamingConvention
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Prefixes
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Postfixes
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> DestinationPrefixes
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> DestinationPostfixes
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<AliasedMember> Aliases
        {
            get { throw new NotImplementedException(); }
        }

        public void RecognizePrefixes(params string[] prefixes)
        {
            throw new NotImplementedException();
        }

        public void RecognizePostfixes(params string[] postfixes)
        {
            throw new NotImplementedException();
        }

        public void RecognizeAlias(string original, string alias)
        {
            throw new NotImplementedException();
        }

        public void RecognizeDestinationPrefixes(params string[] prefixes)
        {
            throw new NotImplementedException();
        }

        public void RecognizeDestinationPostfixes(params string[] postfixes)
        {
            throw new NotImplementedException();
        }

        public void AddGlobalIgnore(string propertyNameStartingWith)
        {
            throw new NotImplementedException();
        }

        public IProfileExpression CreateProfile(string profileName)
        {
            throw new NotImplementedException();
        }

        public void CreateProfile(string profileName, Action<IProfileExpression> initializationExpression)
        {
            throw new NotImplementedException();
        }

        public void AddProfile(Profile profile)
        {
            throw new NotImplementedException();
        }

        public void AddProfile<TProfile>() where TProfile : Profile, new()
        {
            throw new NotImplementedException();
        }

        public void ConstructServicesUsing(Func<Type, object> constructor)
        {
            throw new NotImplementedException();
        }

        public void Seal()
        {
            throw new NotImplementedException();
        }

        public bool HasMapFor(Type source, Type destination)
        {
            return _maps.ContainsKey(source) && _maps[source] == destination;
        }
    }

    public class TestableMappingExpression : IMappingExpression
    {
        public void ConvertUsing<TTypeConverter>()
        {
            
        }

        public void ConvertUsing(Type typeConverterType)
        {
            
        }

        public IMappingExpression WithProfile(string profileName)
        {
            return this;
        }

        public IMappingExpression ForMember(string name, Action<IMemberConfigurationExpression> memberOptions)
        {
            return this;
        }
    }

    public class TestableMappingExpression<T,K> : IMappingExpression<T,K>
    {
        public IMappingExpression<T, K> ForMember(Expression<Func<K, object>> destinationMember, Action<IMemberConfigurationExpression<T>> memberOptions)
        {
            return this;
        }

        public IMappingExpression<T, K> ForMember(string name, Action<IMemberConfigurationExpression<T>> memberOptions)
        {
            return this;
        }

        public void ForAllMembers(Action<IMemberConfigurationExpression<T>> memberOptions)
        {
            
        }

        public IMappingExpression<T, K> Include<TOtherSource, TOtherDestination>() where TOtherSource : T where TOtherDestination : K
        {
            return this;
        }

        public IMappingExpression<T, K> WithProfile(string profileName)
        {
            return this;
        }

        public void ConvertUsing(Func<T, K> mappingFunction)
        {
            
        }

        public void ConvertUsing(ITypeConverter<T, K> converter)
        {
            
        }

        public void ConvertUsing<TTypeConverter>() where TTypeConverter : ITypeConverter<T, K>
        {
            
        }

        public IMappingExpression<T, K> BeforeMap(Action<T, K> beforeFunction)
        {
            return this;
        }

        public IMappingExpression<T, K> BeforeMap<TMappingAction>() where TMappingAction : IMappingAction<T, K>
        {
            return this;
        }

        public IMappingExpression<T, K> AfterMap(Action<T, K> afterFunction)
        {
            return this;
        }

        public IMappingExpression<T, K> AfterMap<TMappingAction>() where TMappingAction : IMappingAction<T, K>
        {
            return this;
        }

        public IMappingExpression<T, K> ConstructUsing(Func<T, K> ctor)
        {
            return this;
        }

        public IMappingExpression<T, K> ConstructUsing(Func<ResolutionContext, K> ctor)
        {
            return this;
        }

        public void As<T1>()
        {
            
        }

        public IMappingExpression<T, K> MaxDepth(int depth)
        {
            return this;
        }

        public IMappingExpression<T, K> ConstructUsingServiceLocator()
        {
            return this;
        }

        public IMappingExpression<K, T> ReverseMap()
        {
            return new TestableMappingExpression<K, T>();
        }

        public IMappingExpression<T, K> ForSourceMember(Expression<Func<T, object>> sourceMember, Action<ISourceMemberConfigurationExpression<T>> memberOptions)
        {
            return this;
        }
    }
}