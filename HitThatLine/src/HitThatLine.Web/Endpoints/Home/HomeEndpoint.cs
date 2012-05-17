using HitThatLine.Web.Endpoints.Home.Models;

namespace HitThatLine.Web.Endpoints.Home
{
    public class HomeEndpoint
    {
         public HomeViewModel Home(HomeInputModel input)
         {
             return new HomeViewModel();
         }
    }
}