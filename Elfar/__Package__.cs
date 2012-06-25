using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

[assembly: PreApplicationStartMethod(typeof(Elfar.__Package__), "Run")]
namespace Elfar
{
    using Views;

    public static class __Package__
    {
        public static void Run()
        {
            var engine = new Engine(typeof(__Package__).Assembly);
            ViewEngines.Engines.Insert(0, engine);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}