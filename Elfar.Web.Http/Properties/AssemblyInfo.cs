using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using Elfar.Web.Http.Properties;

[assembly: AssemblyTitle("Elfar.WebApi")]
[assembly: AssemblyDescription("Error Logging Filter and Route (ELFAR) for ASP.NET WebApi")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Beyond395 Limited")]
[assembly: AssemblyProduct("Elfar.WebApi")]
[assembly: AssemblyCopyright("© 2012–2014 Beyond395 Limited")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("d58e9b84-8f17-4b3b-987f-e11824ca7205")]
[assembly: AssemblyVersion("2.0.*")]
[assembly: AssemblyFileVersion("2.0")]
[assembly: PreApplicationStartMethod(typeof(AssemblyInfo), "Init")]

namespace Elfar.Web.Http.Properties
{
    public static class AssemblyInfo
    {
        public static void Init()
        {
            var config = GlobalConfiguration.Configuration;
            config.Filters.Add(new ErrorLogFilter());
        }
    }
}