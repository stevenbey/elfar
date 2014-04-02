using System.Web.Mvc;

namespace Elfar.Mvc.ActionResults
{
    internal abstract class ErrorLogResult : ActionResult
    {
        public sealed override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(Content);
        }

        protected abstract string Content { get; }
    }
}