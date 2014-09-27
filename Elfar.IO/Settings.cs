using System;
using System.IO;
using System.Web.Hosting;

namespace Elfar.IO
{
    public class Settings : Elfar.Settings
    {
        public string FilePath
        {
            get { return filePath ?? (FilePath = this["FilePath"]); }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                if (value.StartsWith("~/")) value = HostingEnvironment.MapPath(value);
                else if (value.StartsWith(".")) value = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value.Substring(2));
                else if (value.StartsWith(dataDirectoryMacroString))
                {
                    var index = dataDirectoryMacroString.Length;
                    if (value[index] == 92) index++;
                    value = Path.Combine(DataDirectory, value.Substring(index));
                }
                filePath = value;
            }
        }

        static string DataDirectory
        {
            get
            {
                var currentDomain = AppDomain.CurrentDomain;
                var dataDirectory = currentDomain.GetData(dataDirectoryMacroString.Trim('|')) as string;
                if (String.IsNullOrEmpty(dataDirectory)) dataDirectory = currentDomain.BaseDirectory;
                return dataDirectory;
            }
        }

        string filePath;

        const string dataDirectoryMacroString = "|DataDirectory|";
    }
}