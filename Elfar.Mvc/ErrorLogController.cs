using System.Web.Mvc;

namespace Elfar.Mvc
{
    class ErrorLogController : AsyncController
    {
        public ViewResult Default()
        {
            return View();
        }
        public ErrorLogsResult ErrorLogs()
        {
            return new ErrorLogsResult();
        }
        public void Test()
        {
            throw new TestException();
        }

        internal class ErrorLogsResult : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";
                response.Write("[" + string.Join(",", ErrorLogProvider.All) + "]");
            }
        }
    }
}