using System.Configuration;
using System.Web;

namespace Elfar
{
    public class Settings
    {
        public string Application
        {
            get
            {
                return application ?? (application = this["Application"]);
            }
            set { application = value; }
        }

        protected string this[string name]
        {
            get { return ConfigurationManager.AppSettings["elfar:" + name]; }
        }

        internal static string AppDomainAppId
        {
            get { return string.Concat("[", HttpRuntime.AppDomainAppId.Trim('/'), "]"); }
        }
        internal static string AppDomainAppVirtualPath
        {
            get
            {
                var appDomainAppVirtualPath = (HttpRuntime.AppDomainAppVirtualPath ?? "").Trim('/');
                return string.IsNullOrWhiteSpace(appDomainAppVirtualPath) ? null : string.Concat("[", appDomainAppVirtualPath, "]");
            }
        }

        string application;
    }
}