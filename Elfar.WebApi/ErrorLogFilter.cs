using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;

using errorlog = Elfar.ErrorLog;

namespace Elfar.WebApi
{
    public class ErrorLogFilter
        : ExceptionFilterAttribute
    {
        public ErrorLogFilter(IErrorLogProvider provider, params IErrorLogPlugin[] plugins)
        {
            this.provider = provider;
            this.plugins = (plugins ?? new IErrorLogPlugin[0]).ToList();
            if(Settings == null) Settings = new ErrorLogFilterSettings();
        }
        
        public override void OnException(
            HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (Exclude(actionExecutedContext)) return;

            var errorLog = new errorlog(provider.Application, exception);

            if (!(exception is ErrorLogException)) TryExecute(provider.Save, errorLog);

            plugins.ForEach(p => TryExecute(p.Execute, errorLog));
        }

        static bool Exclude(HttpActionExecutedContext actionExecutedContext)
        {
            return Settings.Exclude != null && Settings.Exclude(actionExecutedContext);
        }
        static void TryExecute(Action<errorlog> action, errorlog errorLog)
        {
            try { action(errorLog); }
            catch (Exception) { }
        }

        public static ErrorLogFilterSettings Settings { get; set; }

        readonly List<IErrorLogPlugin> plugins;
        readonly IErrorLogProvider provider;
    }
}