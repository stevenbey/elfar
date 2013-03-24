using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Elfar
{
    public class ErrorLogFilter : FilterAttribute, IExceptionFilter
    {
        public ErrorLogFilter(IErrorLogProvider provider, params IErrorLogPlugin[] plugins)
        {
            this.provider = provider;
            this.plugins = (plugins ?? new IErrorLogPlugin[0]).ToList();
            if(Settings == null) Settings = new ErrorLogFilterSettings();
        }
        
        public void OnException(ExceptionContext exceptionContext)
        {
            if(Exclude(exceptionContext)) return;

            var errorLog = new MvcErrorLog(provider.Application, exceptionContext).ToErrorLog();

            if (!(exceptionContext.Exception is ErrorLogException)) TryExecute(provider.Save, errorLog);

            plugins.ForEach(p => TryExecute(p.Execute, errorLog));
        }
        
        static bool Exclude(ExceptionContext exceptionContext)
        {
            return Settings.Exclude != null && Settings.Exclude(exceptionContext);
        }
        static void TryExecute(Action<ErrorLog> action, ErrorLog errorLog)
        {
            try { action(errorLog); }
            catch (Exception) { }
        }

        public static ErrorLogFilterSettings Settings { get; set; }

        readonly List<IErrorLogPlugin> plugins;
        readonly IErrorLogProvider provider;
    }
}