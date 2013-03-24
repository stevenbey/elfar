using System.Collections.Generic;
using System.Web.Http.Routing;

namespace Elfar.WebApi
{
    public class ErrorLogRouteSettings
    {
        public ErrorLogRouteSettings()
        {
            Constraints = new List<IHttpRouteConstraint>();
        }

        public List<IHttpRouteConstraint> Constraints { get; private set; }
    }
}