using System.Web.Mvc;
using Elfar.Mvc.ActionResults;

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
        [HttpPost]
        public DashboardResult Dashboard()
        {
            return new DashboardResult();
        }
        public EmbeddedResourceResult Resource(string filename, string ext)
        {
            return new EmbeddedResourceResult(filename, ext);
        }
        public void Test()
        {
            throw new TestException();
        }
    }
}