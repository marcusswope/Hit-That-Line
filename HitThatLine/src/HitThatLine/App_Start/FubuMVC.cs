using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap;

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
        }


        private static void SetupContainer(ConfigurationExpression x)
        {
        }
    }
}