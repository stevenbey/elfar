using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Elfar.Web.Mvc;
using Elfar.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(Elfar.Tests.UI.Elfar), "Init")]

namespace Elfar.Tests.UI
{
    public static class Elfar
    {
        public static void Init()
        {
            ErrorLogRoute.Constraints = new[] {new LocalConstraint()};
            RegisterRoutes(RouteTable.Routes);
            UpdateViewEngines(ViewEngines.Engines);
        }

        static void FilterLocationFormats(this RazorViewEngine engine, params Expression<Func<RazorViewEngine, string[]>>[] expressions)
        {
            var type = engine.GetType();
            expressions.ForEach(expression =>
            {
                var property = type.GetProperty(((MemberExpression) expression.Body).Member.Name);
                property.SetValue(engine, ((string[]) property.GetValue(engine, null)).Where(f => f.EndsWith("cshtml")).ToArray(), null);
            });
        }
        static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach(var item in collection) action(item);
        }
        static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("", "", new { controller = "Default", action = "Default" });
        }
        static void UpdateViewEngines(ViewEngineCollection engines)
        {
            engines.Remove(engines.First(e => e is WebFormViewEngine));
            engines.OfType<RazorViewEngine>().ForEach(engine => engine.FilterLocationFormats
            (
                e => e.AreaMasterLocationFormats,
                e => e.AreaPartialViewLocationFormats,
                e => e.AreaViewLocationFormats,
                e => e.FileExtensions,
                e => e.MasterLocationFormats,
                e => e.PartialViewLocationFormats,
                e => e.ViewLocationFormats
            ));
        }
    }
}