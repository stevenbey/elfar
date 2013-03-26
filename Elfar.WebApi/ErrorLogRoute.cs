using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using Elfar.WebApi.RouteConstraints;

namespace Elfar.WebApi
{
    [Export]
    public class ErrorLogRoute : HttpRoute
    {
        static ErrorLogRoute()
        {
            Settings = new ErrorLogRouteSettings();
        }
        
        [ImportingConstructor]
        public ErrorLogRoute(IErrorLogProvider provider)
            : base(Url,
                   new HttpRouteValueDictionary(new { controller = "ErrorLog", id = RouteParameter.Optional }),
                   new HttpRouteValueDictionary(new { id = new GuidConstraint() }),
                   new HttpRouteValueDictionary(new { provider }))
        {
            foreach(var constraint in Settings.Constraints.Where(constraint => constraint != null))
            {
                Constraints.Add(string.Empty, constraint);
            }
        }

        public static ErrorLogRouteSettings Settings { get; set; }

        internal const string Url = "api/elfar/{id}";
    }
}