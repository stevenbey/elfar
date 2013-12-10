using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfar
{
    class ErrorLogPlugins
    {
        static ErrorLogPlugins()
        {
            plugins = Components.CreateMany<IErrorLogPlugin>().ToList();
        }
        
        public static void Execute(ErrorLog errorLog)
        {
            foreach(var plugin in plugins)
            {
                try { plugin.Execute(errorLog); }
                catch(Exception) { }
            }
        }

        static readonly List<IErrorLogPlugin> plugins;
    }
}
