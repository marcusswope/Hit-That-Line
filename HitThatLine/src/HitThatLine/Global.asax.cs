using System.Web;
using HitThatLine.App_Start;

namespace HitThatLine
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AppStartFubuMVC.Start();
        }
    }
}
