using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Elfar.Mvc.Views;

[assembly: AssemblyTitle("Elfar.Mvc")]
[assembly: AssemblyDescription("Error Logging Filter and Route (ELFAR)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Beyond395 Limited")]
[assembly: AssemblyProduct("Elfar.Mvc")]
[assembly: AssemblyCopyright("© 2012–2014 Beyond395 Limited")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("369b6b75-7b12-433e-857a-c124e069d590")]
[assembly: AssemblyVersion("2.0.*")]
[assembly: AssemblyFileVersion("2.0")]
[assembly: PreApplicationStartMethod(typeof(Elfar.Mvc.Properties.AssemblyInfo), "Init")]

namespace Elfar.Mvc.Properties
{
    public static class AssemblyInfo
    {
        public static void Init()
        {
            ViewEngines.Engines.Insert(0, new Engine());
            GlobalFilters.Filters.Add(new ErrorLogFilter());
            RouteTable.Routes.Insert(0, new ErrorLogRoute());
        }

        static T GetAttribute<T>(this ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
        {
            return provider.GetCustomAttributes(typeof(T), inherit).OfType<T>().FirstOrDefault();
        }

        public static string Value
        {
            get
            {
                return string.Format(
                    "{0} v{1} · {2} · All rights reserved",
                    assembly.GetAttribute<AssemblyDescriptionAttribute>().Description,
                    Version,
                    assembly.GetAttribute<AssemblyCopyrightAttribute>().Copyright
                );
            }
        }

        static Version Version
        {
            get
            {
                return assembly.GetName().Version;
            }
            
        }

        static readonly Assembly assembly = typeof (AssemblyInfo).Assembly;
    }
}