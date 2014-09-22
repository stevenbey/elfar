#pragma warning disable 649
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace Elfar
{
    public class Settings
    {
        protected string GetAppSetting(string name)
        {
            return AppSettings["elfar:" + name];
        }

        public string Application
        {
            get
            {
                return application ?? (application = GetAppSetting("Application") ?? AppDomainAppVirtualPath ?? AppDomainAppId);
            }
            set { application = value; }
        }

        static string AppDomainAppId
        {
            get { return HttpRuntime.AppDomainAppId.Trim('/'); }
        }
        static string AppDomainAppVirtualPath
        {
            get
            {
                var appDomainAppVirtualPath = (HttpRuntime.AppDomainAppVirtualPath ?? "").Trim('/');
                return string.IsNullOrWhiteSpace(appDomainAppVirtualPath) ? null : appDomainAppVirtualPath;
            }
        }

        NameValueCollection AppSettings
        {
            get { return appSettings ?? (appSettings = ConfigurationManager.AppSettings); }
        }

        string application;
        NameValueCollection appSettings;
    }
}