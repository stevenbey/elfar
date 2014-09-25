using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Elfar.Web.Mvc.Properties;
using VirtualPathProvider = Elfar.Web.Hosting.VirtualPathProvider;

[assembly: AssemblyTitle("Elfar.Mvc")]
[assembly: AssemblyDescription("Error Logging Filter and Route (ELFAR) for ASP.NET MVC")]
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
[assembly: PreApplicationStartMethod(typeof(AssemblyInfo), "Init")]

namespace Elfar.Web.Mvc.Properties
{
    public static class AssemblyInfo
    {
        public static void Init()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new VirtualPathProvider());
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
                    "{0} [v{1}] · {2} · All rights reserved",
                    assembly.GetAttribute<AssemblyDescriptionAttribute>().Description,
                    assembly.GetName().Version.ToString(2),
                    assembly.GetAttribute<AssemblyCopyrightAttribute>().Copyright
                );
            }
        }

        static readonly Assembly assembly = typeof (AssemblyInfo).Assembly;
    }
}