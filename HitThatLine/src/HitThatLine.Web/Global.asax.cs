using System.Web;
using HitThatLine.Web.App_Start;

namespace HitThatLine.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AppStartFubuMVC.Start();
        }
    }
}
