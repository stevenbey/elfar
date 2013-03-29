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
        [ImportingConstructor]
        public ErrorLogRoute(IErrorLogProvider provider)
            : base(Url,
                   new HttpRouteValueDictionary(new { controller = "ErrorLog", id = RouteParameter.Optional }),
                   new HttpRouteValueDictionary(new { id = new GuidConstraint() }),
                   new HttpRouteValueDictionary(new { provider }))
        {
            if(Settings.Constraints == null) return;
            foreach(var constraint in Settings.Constraints.Where(c => c != null))
            {
                Constraints.Add(string.Empty, constraint);
            }
        }

        internal const string Url = "api/elfar/{id}";
    }
}