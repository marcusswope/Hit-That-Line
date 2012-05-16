using FubuMVC.Core;
using FubuMVC.Razor;
using HitThatLine.Infrastructure.Behaviors;
using HitThatLine.Infrastructure.Conventions;
using HitThatLine.Infrastructure.Conventions.Html;

namespace HitThatLine.Infrastructure
{
    public class HitThatLineRegistry : FubuRegistry
    {
        public HitThatLineRegistry()
        {
            IncludeDiagnostics(true);

            Applies.ToThisAssembly();

            Policies.WrapBehaviorChainsWith<RavenBehavior>();

            Actions.IncludeTypesNamed(x => x.EndsWith("Endpoint"));

            Routes.Configure();
            
            Import<RazorEngineRegistry>();
            
            Views.TryToAttachWithDefaultConventions();
            
            HtmlConvention<InputConventions>();
        }
    }
}
