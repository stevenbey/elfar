using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
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
            return View();
        }
        public ContentResult Detail(Guid id)
        {
            return Content(ErrorLogProvider.Get(id), "application/json");
        }
        [HttpPost]
        public JsonResult Detail(Guid id, string detail)
        {
            return Json(ErrorLogProvider.Save(id, detail));
        }
        public FileStreamResult Script()
        {
            return FileStream("Elfar.Web.Mvc/Resources/Script.min.js", "text/javascript");
        }
        public FileStreamResult Styles()
        {
            return FileStream("Elfar.Web.Mvc/Resources/Styles.min.css", "text/css");
        }
        public ContentResult Summaries()
        {
            return Content(ErrorLogProvider.Summaries, "application/json");
        }
        public void Test()
        {
            throw new TestException();
        }

        FileStreamResult FileStream(string virtualPath, string contentType)
        {
            return File(HostingEnvironment.VirtualPathProvider.GetFile(virtualPath).Open(), contentType);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        readonly RequestContext requestContext;
    }
}