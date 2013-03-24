using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using Elfar.WebApi.RouteConstraints;

namespace Elfar.WebApi
{
    public class ErrorLogRoute : HttpRoute
    {
        public ErrorLogRoute(IErrorLogProvider provider)
            : base(Url,
                   new HttpRouteValueDictionary(new { controller = "ErrorLog", id = RouteParameter.Optional }),
                   new HttpRouteValueDictionary(new { id = new GuidConstraint() }),
                   new HttpRouteValueDictionary(new { provider }))
        {
            if(Settings == null) Settings = new ErrorLogRouteSettings();

            foreach(var constraint in Settings.Constraints.Where(constraint => constraint != null))
            {
                Constraints.Add(string.Empty, constraint);
            }
        }

        public static ErrorLogRouteSettings Settings { get; set; }

        internal const string Url = "api/elfar/{id}";
    }
}