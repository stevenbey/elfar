using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace Elfar
{
    static class Components
    {
        static Components()
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsElfarAssembly).ToArray();
            compositionContainer = new CompositionContainer(new AggregateCatalog(assemblies.Select(a => new AssemblyCatalog(a))));
        }

        public static T Create<T>()
        {
            return compositionContainer.GetExportedValueOrDefault<T>();
        }
        public static IEnumerable<T> CreateMany<T>()
        {
            return compositionContainer.GetExportedValues<T>();
        }

        static bool IsElfarAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Elfar", StringComparison.OrdinalIgnoreCase);
        }

        public static Assembly[] Assemblies
        {
            get { return assemblies; }
        }

        static readonly Assembly[] assemblies;
        static readonly CompositionContainer compositionContainer;
    }
}