using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elfar.Web.Mvc
{
    class ErrorLogController : AsyncController, IHttpAsyncHandler
    {
        public ErrorLogController(RequestContext requestContext)
        {
            this.requestContext = requestContext;
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return BeginExecute(requestContext, cb, extraData);
        }
        public void EndProcessRequest(IAsyncResult result)
        {
            EndExecute(result);
        }
        public void ProcessRequest(HttpContext context)
        {
            Execute(requestContext);
        }

        public ViewResult Default()
        {
            return View((object) ErrorLogProvider.Summaries);
        }
        public ContentResult Detail(Guid id)
        {
            return new ContentResult { Content = ErrorLogProvider.Get(id), ContentType = "application/json" };
        }
        [HttpPost]
        public JsonResult Detail(Guid id, string detail)
        {
            return new JsonResult { Data = ErrorLogProvider.Save(id, detail) };
        }
        public FileStreamResult Resource(string name)
        {
            return new FileStreamResult(name);
        }
        public FileStreamResult Script()
        {
            return new FileStreamResult("Elfar.Web.Mvc.Resources.Script.js");
        }
        public FileStreamResult Styles()
        {
            return new FileStreamResult("Elfar.Web.Mvc.Resources.Styles.css");
        }
        public void Test()
        {
            throw new TestException();
        }

        public bool IsReusable
        {
            get { return false; }
        }

        readonly RequestContext requestContext;
    }
}