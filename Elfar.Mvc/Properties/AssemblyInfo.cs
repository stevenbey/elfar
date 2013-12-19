using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Elfar.Mvc.Resources;
using Elfar.Mvc.Views;

[assembly: AssemblyTitle("Elfar.Mvc")]
[assembly: AssemblyDescription("Error Logging Filter and Route (ELFAR) for ASP.NET MVC")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Beyond395 Limited")]
[assembly: AssemblyProduct("Elfar.Mvc")]
[assembly: AssemblyCopyright("Copyright © Beyond395 Limited 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("369b6b75-7b12-433e-857a-c124e069d590")]
[assembly: AssemblyVersion("2.0.*")]
[assembly: AssemblyFileVersion("2.0")]

[assembly: PreApplicationStartMethod(typeof(Elfar.Mvc.Properties.Module), "Init")]
namespace Elfar.Mvc.Properties
{
    public static class Module
    {
        public static void Init()
        {
            ViewEngines.Engines.Insert(0, new Engine());
            GlobalFilters.Filters.Add(new ErrorLogFilter());
            RouteTable.Routes.Insert(0, new ErrorLogRoute());
            HostingEnvironment.RegisterVirtualPathProvider(new Provider());
        }
    }
}