using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration;
using FubuCore.Reflection;

namespace HitThatLine.Infrastructure.Security
{
    public class AccessAuthorizationConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                 .Where(action => action.HandlerType.HasAttribute<OnlyAllowRolesAttribute>() || 
                                  action.Method.HasAttribute<OnlyAllowRolesAttribute>())
                 .Each(action => action
                     .ParentChain().Authorization
                     .AddPolicy(typeof(AccessAuthorizationPolicy<>).MakeGenericType(action.HandlerType)));
        }
    }
}