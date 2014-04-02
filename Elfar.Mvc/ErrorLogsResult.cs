using System.Web.Mvc;

namespace Elfar.Mvc
{
    internal sealed class ErrorLogResult : ActionResult
    {
        public ErrorLogResult(string content)
        {
            this.content = content;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(content);
        }

        readonly string content;
    }
}