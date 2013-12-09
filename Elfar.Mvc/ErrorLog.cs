using System.Web.Mvc;

namespace Elfar.Mvc
{
    class ErrorLog : Elfar.ErrorLog
    {
        public ErrorLog(string application, ExceptionContext exceptionContext) : base(application, (Json) exceptionContext) {}
    }
}