[assembly: WebActivator.PreApplicationStartMethod(typeof(Elfar.WebApi.Host.App_Start.Elfar), "Init")]
namespace Elfar.WebApi.Host.App_Start
{
    public static class Elfar
    {
        public static void Init()
        {
            //global::Elfar.WebApi.Settings.Constraints = null;
            //global::Elfar.WebApi.Settings.Exclude = null;
        }
    }
}