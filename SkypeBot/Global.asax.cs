using System.Web.Http;

namespace Bot.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BlobCache.ApplicationName = "TestSkypeBot";
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
