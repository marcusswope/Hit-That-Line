using FubuMVC.Core.Registration;
using System.Linq;
using FubuMVC.Core.Registration.Nodes;
using System.Collections.Generic;
using FubuCore;

namespace HitThatLine.Infrastructure.Validation
{
    public class ValidationConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                .Where(x => x.HasInput && x.InputType().CanBeCastTo<IValidatedCommand>())
                .Each(x => x.AddBefore(new Wrapper(typeof(ValidationBehavior<>).MakeGenericType(x.InputType()))));
        }
    }
}