using FubuMVC.Core;
using FubuMVC.Spark;

namespace HitThatLine.Web
{
    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            IncludeDiagnostics(true);

            Actions
                .IncludeTypesNamed(x => x.EndsWith("Endpoint"));
            
            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodSuffix("Html")
                .RootAtAssemblyNamespace();

            Views
                .TryToAttachWithDefaultConventions();

            this.UseSpark();
        }
    }
}