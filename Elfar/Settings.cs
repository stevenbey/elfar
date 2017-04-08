using System.Configuration;
using System.Web;

namespace Elfar
{
    public class Settings
    {
        private string application;

        public string Application
        {
            get
            {
                return this.application ?? (this.application = this["Application"]);
            }

            set
            {
                this.application = value;
            }
        }

        protected string this[string name] => ConfigurationManager.AppSettings["elfar:" + name];

        internal static string AppDomainAppId => string.Concat("[AppDomainAppId: ", HttpRuntime.AppDomainAppId.Trim('/'), "]");

        internal static string AppDomainAppVirtualPath
        {
            get
            {
                var appDomainAppVirtualPath = (HttpRuntime.AppDomainAppVirtualPath ?? "").Trim('/');
                return string.IsNullOrWhiteSpace(appDomainAppVirtualPath) ? null : string.Concat("[AppDomainAppVirtualPath: ", appDomainAppVirtualPath, "]");
            }
        }
    }
}