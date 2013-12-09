using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Elfar.Mvc
{
    public class ErrorLogFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if(Exclude(exceptionContext)) return;

            var errorLog = new MvcErrorLog(Provider.Application, exceptionContext).ToErrorLog();

            if(!(exceptionContext.Exception is ErrorLogException)) TryExecute(Provider.Save, errorLog);

            foreach(var plugin in Plugins)
            {
                TryExecute(plugin.Execute, errorLog);
            }
        }

        static bool Exclude(ExceptionContext exceptionContext)
        {
            return Settings.Exclude != null && Settings.Exclude(exceptionContext);
        }
        static void TryExecute(Action<ErrorLog> action, ErrorLog errorLog)
        {
            try { action(errorLog); }
            catch(Exception) { }
        }

        IEnumerable<IErrorLogPlugin> Plugins
        {
            get { return plugins ?? (plugins = new List<IErrorLogPlugin>(Components.CreateMany<IErrorLogPlugin>())); }
        }
        IErrorLogProvider Provider
        {
            get { return provider ?? (provider = Components.Create<IErrorLogProvider>()); }
        }

        IEnumerable<IErrorLogPlugin> plugins;
        IErrorLogProvider provider;
    }
}