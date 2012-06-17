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
                .IgnoreControllerNamesEntirely()
                .ConstrainToHttpMethod(x => x.InputType().Name.EndsWith("Request"), "GET")
                .ConstrainToHttpMethod(x => x.InputType().Name.EndsWith("Command"), "POST")
                .HomeIs<HomeRequest>();
        }
    }
}