#pragma warning disable 649

using System.Collections.Specialized;
using System.Configuration;

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
            get { return application ?? (application = GetAppSetting("Application")); }
            set { application = value; }
        }
        public string FilePath
        {
            get { return filePath ?? (filePath = GetAppSetting("FilePath")); }
            set { filePath = value; }
        }

        NameValueCollection AppSettings
        {
            get { return appSettings ?? (appSettings = ConfigurationManager.AppSettings); }
        }

        string application;
        NameValueCollection appSettings;
        string filePath;
    }
}