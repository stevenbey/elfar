using System.Linq;
using System.Web.Mvc;
using Elfar.Mvc.Resources;

namespace Elfar.Mvc
{
    class ErrorLogController : AsyncController
    {
        public ViewResult Default()
        {
            return View();
        }
        [HttpPost]
        public ErrorLogResult Details(int id)
        {
            return new ErrorLogResult(ErrorLogProvider.All.Single(i => i.ID == id).Detail);
        }
        [HttpPost]
        public ErrorLogResult Dashboard()
        {
            return new ErrorLogResult("[" + string.Join(",", ErrorLogProvider.All.Select(i => i.Summary)) + "]");
        }
        public Result Resource(string filename, string ext)
        {
            return new Result(filename, ext);
        }
        public void Test()
        {
            throw new TestException();
        }
    }
}