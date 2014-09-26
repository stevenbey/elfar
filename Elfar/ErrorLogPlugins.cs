using System.Linq;

namespace Elfar
{
    class ErrorLogPlugins
    {
        static ErrorLogPlugins()
        {
            plugins = Composition.CreateMany<IErrorLogPlugin>().ToArray();
        }
        
        internal static void Execute(ErrorLog errorLog)
        {
            foreach(var plugin in plugins) plugin.Execute(errorLog);
        }

        static readonly IErrorLogPlugin[] plugins;
    }
}
