using System;
using System.Web.Hosting;

namespace Elfar.IO
{
    public class Settings : Elfar.Settings
    {
        static string Combine(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }
        static string Remainder(string value, int index)
        {
            if (value[index] == 92) index++;
            return value.Substring(index);
        }
        static string Resolve(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.StartsWith("..")) value = DefaultPath;
            else if (value.StartsWith(".")) value = Combine(AppDomain.CurrentDomain.BaseDirectory, Remainder(value, 2));
            else if (value.StartsWith("~/")) value = HostingEnvironment.MapPath(value);
            else if (value.StartsWith(dataDirectoryMacroString)) value = Combine(DataDirectory, Remainder(value, dataDirectoryMacroString.Length));
            // ReSharper disable once PossibleNullReferenceException
            return System.IO.Path.GetDirectoryName(value.Replace("/", @"\"));
        }

        public string Path
        {
            get { return Resolve(path ?? (Path = this["Path"])); }
            set { path = value; }
        }

        static string DataDirectory
        {
            get
            {
                var currentDomain = AppDomain.CurrentDomain;
                var dataDirectory = currentDomain.GetData(dataDirectoryMacroString.Trim('|')) as string;
                if (string.IsNullOrEmpty(dataDirectory)) dataDirectory = currentDomain.BaseDirectory;
                return dataDirectory;
            }
        }
        static string DefaultPath
        {
            get { return Combine(DataDirectory, "Elfar"); }
        }

        const string dataDirectoryMacroString = "|DataDirectory|";
        string path;
    }
}