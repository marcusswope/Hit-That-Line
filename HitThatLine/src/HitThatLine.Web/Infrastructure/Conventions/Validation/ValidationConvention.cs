using FubuMVC.Core.Registration;
using System.Linq;
using FubuMVC.Core.Registration.Nodes;
using HitThatLine.Web.Infrastructure.Behaviors;
using System.Collections.Generic;

namespace HitThatLine.Web.Infrastructure.Conventions.Validation
{
    public class ValidationConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                .Where(x => x.HasInput && x.InputType().IsClass)
                .Each(x => x.AddBefore(new Wrapper(typeof(ValidationBehavior<>).MakeGenericType(x.InputType()))));
        }
    }
}