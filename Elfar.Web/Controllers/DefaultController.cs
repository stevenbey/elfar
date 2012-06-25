using System.Web.Mvc;

namespace Elfar.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Default()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Default(string password)
        {
            return Default();
        }
    }
}