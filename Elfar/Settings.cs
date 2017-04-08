using System.Configuration;
using System.Web;

namespace Elfar
{
    public class Settings
    {
        private string application;

        public string Application
        {
            get => this.application ?? (this.application = this[nameof(Application)]);

            set => this.application = value;
        }

        protected string this[string name] => ConfigurationManager.AppSettings["elfar:" + name];
        protected string this[string name, string plugin] => this[plugin + "." + name];

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