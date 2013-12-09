using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Elfar.Mvc
{
    public class ErrorLogFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if (Exclude(exceptionContext)) return;

            var errorLog = new ErrorLog(Provider.Application, exceptionContext);

            if(!(exceptionContext.Exception is ErrorLogException)) TryExecute(Provider.Save, errorLog);

            foreach(var plugin in Plugins)
            {
                TryExecute(plugin.Execute, errorLog);
            }
        }

        static void TryExecute(Action<ErrorLog> action, ErrorLog errorLog)
        {
            try { action(errorLog); }
            catch (Exception) { }
        }

        public static Predicate<ExceptionContext> Exclude
        {
            get { return exclude ?? (exclude = (c => false)); }
            set { exclude = value; }
        }

        IEnumerable<IErrorLogPlugin> Plugins
        {
            get { return plugins ?? (plugins = new List<IErrorLogPlugin>(Components.CreateMany<IErrorLogPlugin>())); }
        }
        IErrorLogProvider Provider
        {
            get { return provider ?? (provider = Components.Create<IErrorLogProvider>()); }
        }

        static Predicate<ExceptionContext> exclude;
        IEnumerable<IErrorLogPlugin> plugins;
        IErrorLogProvider provider;
    }
}