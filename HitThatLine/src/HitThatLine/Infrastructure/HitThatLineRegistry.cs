using FubuMVC.Core;
using FubuMVC.Razor;
using HitThatLine.Infrastructure.Html;
using HitThatLine.Infrastructure.Raven;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Infrastructure.Validation;

namespace HitThatLine.Infrastructure
{
    public class HitThatLineRegistry : FubuRegistry
    {
        public HitThatLineRegistry()
        {
            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly();

            Policies
                .Add<AccessAuthorizationConvention>()
                .WrapBehaviorChainsWith<UserAccountBehavior>()
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
