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

            var provider = Provider;
            var plugins = Plugins;
            var config = GlobalConfiguration.Configuration;

            config.Filters.Add(new ErrorLogFilter(provider, plugins));
            config.Routes.Add("elfar", new ErrorLogRoute(provider));
        }

        static bool IsElfarAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Elfar", StringComparison.OrdinalIgnoreCase);
        }

        static IErrorLogPlugin[] Plugins
        {
            get
            {
                return compositionContainer.GetExports<IErrorLogPlugin>("Plugin").Select(p => p.Value).ToArray();
            }
        }
        static IErrorLogProvider Provider
        {
            get
            {
                try
                {
                    return compositionContainer.GetExports<IErrorLogProvider>("Provider").Single().Value;
                }
                catch(Exception)
                {
                    throw new Exception("Multiple ErrorLogProviders found.");
                }
            }
        }

        static readonly Assembly[] assemblies; 
        static readonly CompositionContainer compositionContainer;
    }
}