using Bottles;
using FluentValidation;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.StructureMap;
using HitThatLine.Infrastructure;
using HitThatLine.Infrastructure.TaskExecution;
using StructureMap;
using System.Collections.Generic;

namespace HitThatLine.App_Start
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
            TaskExecutor.ExceptionHandler = e => {};
        }


        private static void SetupContainer(ConfigurationExpression map)
        {
            map.Scan(x =>
                         {
                             x.TheCallingAssembly();
                             x.WithDefaultConventions();
                             x.LookForRegistries();
                             x.AddAllTypesOf<IPropertyBinder>();
                             x.ConnectImplementationsToTypesClosing(typeof (IValidator<>));

                             PackageRegistry.PackageAssemblies.Each(x.Assembly);
                         });

            map.For<IContinuationDirector>().Use<ContinuationHandler>();
        }
    }
}