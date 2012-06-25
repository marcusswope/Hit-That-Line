using FubuMVC.Core.Registration.DSL;
using HitThatLine.Infrastructure.Raven;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Infrastructure.TaskExecution;
using HitThatLine.Infrastructure.Validation;

namespace HitThatLine.Infrastructure
{
    public static class PolicyConventions
    {
         public static void Configure(this PoliciesExpression policies)
         {
             policies
                 .Add<ValidationConvention>()
                 .WrapBehaviorChainsWith<UserAccountBehavior>()
                 .WrapBehaviorChainsWith<TaskExecutorBehavior>()
                 .Add<AccessAuthorizationConvention>()
                 .WrapBehaviorChainsWith<RavenBehavior>();
         }
    }
}