using System;
using System.Web.Http.Filters;

using errorlog = Elfar.ErrorLog;

namespace Elfar.WebApi
{
    public class ErrorLogFilter
        : ExceptionFilterAttribute
    {
        public ErrorLogFilter(
            IErrorLogProvider provider,
            Predicate<HttpActionExecutedContext> exclude = null,
            IErrorLogMail mail = null,
            IErrorLogTweet tweet = null)
        {
            this.provider = provider;
            this.exclude = exclude;
            this.mail = mail;
            this.tweet = tweet;
        }

        public override void OnException(
            HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (exclude != null && exclude(actionExecutedContext)) return;

            var errorLog = new errorlog(provider.Application, exception);

            if (!(exception is ErrorLogException)) Execute(provider.Save, errorLog);

            if (mail != null) Execute(mail.Send, errorLog);
            if (tweet != null) Execute(tweet.Post, errorLog);
        }

        static void Execute(
            Action<errorlog> action,
            errorlog errorLog)
        {
            try { action(errorLog); }
            catch (Exception) { }
        }

        readonly Predicate<HttpActionExecutedContext> exclude;
        readonly IErrorLogMail mail;
        readonly IErrorLogProvider provider;
        readonly IErrorLogTweet tweet;
    }
}