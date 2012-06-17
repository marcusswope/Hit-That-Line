using HitThatLine.Endpoints.Home.Models;

namespace HitThatLine.Endpoints.Home
{
    public class HomeEndpoint
    {
         public HomeViewModel Home(HomeRequest input)
         {
             return new HomeViewModel();
         }
    }
}