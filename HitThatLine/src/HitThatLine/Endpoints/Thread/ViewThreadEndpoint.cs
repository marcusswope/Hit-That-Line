using FubuMVC.Core.Continuations;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Endpoints.Thread.Models;

namespace HitThatLine.Endpoints.Thread
{
    public class ViewThreadEndpoint
    {
         public FubuContinuation Thread(ViewThreadRequest request)
         {
             return FubuContinuation.RedirectTo<HomeRequest>();
         }
    }
}