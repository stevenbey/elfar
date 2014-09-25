using System;
using System.Linq;
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
            return View((object)string.Join(",", ErrorLogProvider.All.Select(i => i.Summary)));
        }
        public ErrorLogResult Details(int id)
        {
            return new ErrorLogResult(ErrorLogProvider.All.Single(i => i.ID == id).Detail);
        }
        [HttpPost]
        public void Details(int id, string content)
        {
            
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