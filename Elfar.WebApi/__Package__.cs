using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

[assembly: PreApplicationStartMethod(typeof(Elfar.WebApi.__Package__), "Run")]
namespace Elfar.WebApi
{
    public static class __Package__
    {
        static __Package__()
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsElfarAssembly).ToArray();
            compositionContainer = new CompositionContainer(new AggregateCatalog(assemblies.Select(a => new AssemblyCatalog(a))));
        }

        public static void Run()
        {
            if(ErrorLogProvider.Settings == null) ErrorLogProvider.Settings = new ErrorLogProviderSettings();

            var config = GlobalConfiguration.Configuration;
            config.Filters.Add(compositionContainer.GetExport<ErrorLogFilter>().Value);
            config.Routes.Add("elfar", compositionContainer.GetExport<ErrorLogRoute>().Value);
        }

        static bool IsElfarAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Elfar", StringComparison.OrdinalIgnoreCase);
        }

        static readonly Assembly[] assemblies; 
        static readonly CompositionContainer compositionContainer;
    }
}