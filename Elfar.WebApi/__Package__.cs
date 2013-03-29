using System.Web;
using System.Web.Http;

[assembly: PreApplicationStartMethod(typeof(Elfar.WebApi.__Package__), "Run")]
namespace Elfar.WebApi
{
    public static class __Package__
    {
        public static void Run()
        {
            var config = GlobalConfiguration.Configuration;
            config.Filters.Add(Components.Create<ErrorLogFilter>());
            config.Routes.Add("elfar", Components.Create<ErrorLogRoute>());
        }
    }
}