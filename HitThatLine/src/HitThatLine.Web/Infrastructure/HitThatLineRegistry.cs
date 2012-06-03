using FubuMVC.Core;
using FubuMVC.Razor;
using HitThatLine.Web.Infrastructure.Behaviors;
using HitThatLine.Web.Infrastructure.Conventions;
using HitThatLine.Web.Infrastructure.Conventions.Html;
using HitThatLine.Web.Infrastructure.Conventions.Validation;

namespace HitThatLine.Web.Infrastructure
{
    public class HitThatLineRegistry : FubuRegistry
    {
        public HitThatLineRegistry()
        {
            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly();

            Policies
                .WrapBehaviorChainsWith<RavenBehavior>();

            Actions
                .IncludeTypesNamed(x => x.EndsWith("Endpoint"));

            Routes
                .Configure();
            
            Import<RazorEngineRegistry>();
            
            Views
                .TryToAttachWithDefaultConventions();
            
            Assets
                .CombineAllUniqueAssetRequests();

            HtmlConvention<InputConventions>();
            HtmlConvention<LabelConventions>();
            ApplyConvention<ValidationConvention>();
        }
    }
}
