using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace Elfar.Mvc
{
    [Export]
    public class ErrorLogFilter : FilterAttribute //, IExceptionFilter
    {
        static ErrorLogFilter()
        {
            Settings = new ErrorLogFilterSettings();
        }

        [ImportingConstructor]
        public ErrorLogFilter(IErrorLogProvider provider)
        {
            this.provider = provider;
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            if(Exclude(exceptionContext)) return;

            var errorLog = new ErrorLog(provider.Application, exceptionContext).ToErrorLog();

            if(!(exceptionContext.Exception is ErrorLogException)) TryExecute(provider.Save, errorLog);

            foreach(var plugin in Plugins)
            {
                TryExecute(plugin.Execute, errorLog);
            }
        }

        static bool Exclude(ExceptionContext exceptionContext)
        {
            return Settings.Exclude != null && Settings.Exclude(exceptionContext);
        }
        static void TryExecute(Action<Elfar.ErrorLog> action, Elfar.ErrorLog errorLog)
        {
            try { action(errorLog); }
            catch(Exception) { }
        }

        [ImportMany]
        public IErrorLogPlugin[] Plugins { get; set; }
        public static ErrorLogFilterSettings Settings { get; set; }

        readonly IErrorLogProvider provider;
    }
}