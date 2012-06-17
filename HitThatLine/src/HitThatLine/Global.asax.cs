using System.Web;
using HitThatLine.App_Start;

namespace HitThatLine
{
    public class Global : HttpApplication
    {
        public void Application_Start()
        {
            AppStartFubuMVC.Start();
        }
    }
}
