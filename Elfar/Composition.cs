using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace Elfar
{
    static class Composition
    {
        static Composition()
        {
            Assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsElfarAssembly).ToArray();
            container = new CompositionContainer(new AggregateCatalog(Assemblies.Select(a => new AssemblyCatalog(a))));
        }

        //public static T Create<T>()
        //{
        //    return container.GetExportedValueOrDefault<T>();
        //}
        public static IEnumerable<T> CreateMany<T>()
        {
            return container.GetExportedValues<T>();
        }

        static bool IsElfarAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Elfar", StringComparison.OrdinalIgnoreCase);
        }

        public static Assembly[] Assemblies { get; private set; }

        static readonly CompositionContainer container;
    }
}