using Bottles;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using HitThatLine.Web.Infrastructure;
using StructureMap;
using System.Collections.Generic;

namespace HitThatLine.Web.App_Start
{
    public static class AppStartFubuMVC
    {
        public static void Start()
        {
            FubuApplication
                .For<HitThatLineRegistry>()
                .StructureMap(new Container(SetupContainer))
                .Bootstrap();

			PackageRegistry.AssertNoFailures();
        }


        private static void SetupContainer(ConfigurationExpression map)
        {
            map.Scan(x =>
                         {
                             x.TheCallingAssembly();
                             x.WithDefaultConventions();
                             x.LookForRegistries();
                             x.AddAllTypesOf<IPropertyBinder>();

                             PackageRegistry.PackageAssemblies.Each(x.Assembly);
                         });
        }
    }
}