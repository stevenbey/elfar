using System.Linq;
using System.Web.Mvc;

namespace Elfar.Mvc
{
    class ErrorLogController : AsyncController
    {
        public ViewResult Default()
        {
            return View();
        }
        public DetailResult Details(int id)
        {
            return new DetailResult(id);
        }
        public DashboardResult Dashboard()
        {
            return new DashboardResult();
        }
        public void Test()
        {
            throw new TestException();
        }

        internal class DetailResult : ErrorLogsResult
        {
            public DetailResult(int id)
            {
                this.id = id;
            }

            protected override string Content
            {
                get { return ErrorLogProvider.All.Single(i => i.ID == id).Detail; }
            }
            
            readonly int id;
        }
        internal class DashboardResult : ErrorLogsResult
        {
            protected override string Content
            {
                get { return "[" + string.Join(",", ErrorLogProvider.All.Select(i => i.Summary)) + "]"; }
            }
        }
        internal abstract class ErrorLogsResult : ActionResult
        {
            public sealed override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";
<<<<<<< HEAD
                response.Write(Content);
=======
                response.Write("var errors = [" + string.Join(",", ErrorLogProvider.All) + "];");
>>>>>>> 53fd557b0647c227bf3c8480e0cc89a3d0ad00f4
            }
            
            protected abstract string Content { get; }
        }
    }
}