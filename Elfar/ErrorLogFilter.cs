using System;
using System.Web.Mvc;

namespace Elfar
{
    public class ErrorLogFilter
        : FilterAttribute,
          IExceptionFilter
    {
        public ErrorLogFilter(
            IErrorLogProvider provider,
            Predicate<ExceptionContext> exclude = null,
            IErrorLogMail mail = null,
            IErrorLogTweet tweet = null)
        {
            this.provider = provider;
            this.exclude = exclude;
            this.mail = mail;
            this.tweet = tweet;
        }
        
        public void OnException(
            ExceptionContext exceptionContext)
        {
            if(exclude != null && exclude(exceptionContext)) return;

            var errorLog = new MvcErrorLog(provider.Application, exceptionContext).ToErrorLog();

            if (!(exceptionContext.Exception is ErrorLogException)) Execute(provider.Save, errorLog);

            if(mail != null) Execute(mail.Send, errorLog);
            if(tweet != null) Execute(tweet.Post, errorLog);
        }
        
        static void Execute(
            Action<ErrorLog> action,
            ErrorLog errorLog)
        {
            try { action(errorLog); }
            catch (Exception) { }
        }

        readonly Predicate<ExceptionContext> exclude;
        readonly IErrorLogMail mail;
        readonly IErrorLogProvider provider;
        readonly IErrorLogTweet tweet;
    }
}