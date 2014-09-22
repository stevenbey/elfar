using System.Web.Mvc;

namespace Elfar.Tests.UI.Controllers
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