﻿using System;
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

        [Minify]
        public ViewResult Default()
        {
            return View();
        }
        public ContentResult Detail(string id)
        {
            return Content(ErrorLogProvider.Get(id), "application/json");
        }
        [HttpPost]
        public JsonResult Detail(string id, string detail)
        {
            return Json(ErrorLogProvider.Save(id, detail));
        }
        public FileStreamResult Icons(string id)
        {
            return FileStream("Elfar.Web.Mvc/Resources/" + (string.IsNullOrWhiteSpace(id) ? "icons" : id) + ".png", "image/png");
        }
        public ContentResult Script()
        {
            return Content
            (
                string.Concat(
                    HostingEnvironment.VirtualPathProvider.GetFile("Elfar.Web.Mvc/Resources/app.js").ToString(),
                    HostingEnvironment.VirtualPathProvider.GetFile("Elfar.Web.Mvc/Resources/Elfar.min.js").ToString()
                ),
                "text/javascript"
            );
        }
        public FileStreamResult Styles()
        {
            return FileStream("Elfar.Web.Mvc/Resources/Elfar.min.css", "text/css");
        }
        public ContentResult Summaries()
        {
            return Content(ErrorLogProvider.Summaries, "application/json");
        }
        public PartialViewResult Template(string id)
        {
            return PartialView(id);
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