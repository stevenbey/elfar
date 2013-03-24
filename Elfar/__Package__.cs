using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

[assembly: PreApplicationStartMethod(typeof(Elfar.__Package__), "Run")]
namespace Elfar
{
    using Views;

    public static class __Package__
    {
        static __Package__()
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsElfarAssembly).ToArray();
            compositionContainer = new CompositionContainer(new AggregateCatalog(assemblies.Select(a => new AssemblyCatalog(a))));
        }

        public static void Run()
        {
            var engine = new Engine(assemblies);
            ViewEngines.Engines.Insert(0, engine);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);

            if(ErrorLogProvider.Settings == null) ErrorLogProvider.Settings = new ErrorLogProviderSettings();

            var provider = Provider;
            var plugins = Plugins;
            GlobalFilters.Filters.Add(new ErrorLogFilter(provider, plugins));
            RouteTable.Routes.Insert(0, new ErrorLogRoute(provider, plugins));
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