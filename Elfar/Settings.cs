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

        public string FilePath
        {
            get { return filePath ?? (filePath = GetAppSetting("FilePath")); }
        }

        NameValueCollection AppSettings
        {
            get { return appSettings ?? (appSettings = ConfigurationManager.AppSettings); }
        }

        NameValueCollection appSettings;
        string filePath;
    }
}