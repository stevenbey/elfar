using System.Collections.Generic;
using System.Web.Routing;

namespace Elfar
{
    public class ErrorLogRouteSettings
    {
        public ErrorLogRouteSettings()
        {
            Constraints = new List<IRouteConstraint>();
        }

        public List<IRouteConstraint> Constraints { get; private set; }
    }
}