using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elfar.Mvc
{
    public static class Settings
    {
        public static Predicate<ExceptionContext> Exclude { get; set; }
        public static IEnumerable<IRouteConstraint> Constraints { get; set; }
    }
}