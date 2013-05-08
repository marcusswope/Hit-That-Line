using FubuMVC.Core;
using FubuMVC.Razor;
using HitThatLine.Infrastructure.Html;

namespace HitThatLine.Infrastructure
{
    public class HitThatLineRegistry : FubuRegistry
    {
        public HitThatLineRegistry()
        {
            IncludeDiagnostics(true);
            Applies.ToThisAssembly();
            
            Policies.Configure();
            Actions.IncludeTypesNamed(x => x.EndsWith("Endpoint"));
            Routes.Configure();
            
            Import<RazorEngineRegistry>();
            Views.TryToAttachWithDefaultConventions();
            Assets.CombineAllUniqueAssetRequests();
            
            HtmlConvention<InputConventions>();
            HtmlConvention<LabelConventions>();
        }
    }
}
