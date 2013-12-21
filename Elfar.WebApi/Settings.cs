using System;
using System.Collections.Generic;
using System.Web.Http.Filters;
using System.Web.Http.Routing;

namespace Elfar.WebApi
{
    public static class Settings
    {
        public static List<IHttpRouteConstraint> Constraints { get; set; }
    }
}