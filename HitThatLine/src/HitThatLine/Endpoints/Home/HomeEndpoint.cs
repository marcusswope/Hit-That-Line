using HitThatLine.Endpoints.Home.Models;

namespace HitThatLine.Endpoints.Home
{
    public class HomeEndpoint
    {
         public HomeViewModel Home(HomeInputModel input)
         {
             return new HomeViewModel();
         }
    }
}