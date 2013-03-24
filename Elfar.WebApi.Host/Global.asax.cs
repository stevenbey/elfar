using System.Web.Http;
using Elfar.Xml;

namespace Elfar.WebApi.Host
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureApi(GlobalConfiguration.Configuration);
        }
        
        static void ConfigureApi(HttpConfiguration config)
        {
            //var provider = new XmlErrorLogProvider(null, @"D:\Projects\Elfar\Elfar.Web\App_Data\Errors");
            //config.Filters.Add(new ErrorLogFilter(provider));
            //config.Routes.Add("elfar", new ErrorLogRoute(provider));
        }
    }
}