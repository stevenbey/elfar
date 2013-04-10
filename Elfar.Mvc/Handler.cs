using System;
using System.Web;
using System.Web.Mvc.Async;
using System.Web.Routing;

namespace Elfar.Mvc
{
    class Handler : IHttpAsyncHandler
    {
        public Handler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            controller = Components.Create<ErrorLogController>();
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return controller.BeginExecute(requestContext, cb, extraData);
        }
        public void EndProcessRequest(IAsyncResult result)
        {
            controller.EndExecute(result);
        }
        public void ProcessRequest(HttpContext context)
        {
            controller.Execute(requestContext);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        readonly IAsyncController controller;
        readonly RequestContext requestContext;
    }
}