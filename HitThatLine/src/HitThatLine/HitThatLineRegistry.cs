using FubuMVC.Core;
using FubuMVC.Razor;
using HitThatLine.Endpoints.Home.Models;

namespace HitThatLine
{
    public class HitThatLineRegistry : FubuRegistry
    {
        public HitThatLineRegistry()
        {
            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly();

            Actions
                .IncludeTypesNamed(x => x.EndsWith("Endpoint"));

            Routes
                .IgnoreControllerNamespaceEntirely()
                .HomeIs<HomeInputModel>();

            Import<RazorEngineRegistry>();

            Views
                .TryToAttachWithDefaultConventions();
        }
    }
}
