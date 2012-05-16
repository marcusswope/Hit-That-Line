using FubuMVC.Core.Registration.DSL;
using HitThatLine.Endpoints.Home.Models;

namespace HitThatLine.Infrastructure.Conventions
{
    public static class RouteConventions
    {
        public static void Configure(this RouteConventionExpression routes)
        {
            routes
                .IgnoreControllerNamespaceEntirely()
                .IgnoreClassSuffix("Endpoint")
                .ConstrainToHttpMethod(x => x.InputType().Name.EndsWith("Command"), "POST")
                .HomeIs<HomeInputModel>();
        }
    }
}