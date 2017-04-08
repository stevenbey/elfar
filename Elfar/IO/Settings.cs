using System;
using System.Web.Hosting;

namespace Elfar.IO
{
    public class Settings : Elfar.Settings
    {
        private const string DataDirectoryMacroString = "|DataDirectory|";
        private string path;

        public string Path
        {
            get => path ?? (path = Resolve(this[nameof(Path)]));
            set => path = Resolve(value);
        }

        private static string DataDirectory
        {
            get
            {
                var currentDomain = AppDomain.CurrentDomain;
                var dataDirectory = currentDomain.GetData(DataDirectoryMacroString.Trim('|')) as string;
                if (string.IsNullOrEmpty(dataDirectory)) dataDirectory = currentDomain.BaseDirectory;
                return dataDirectory;
            }
        }

        private static string DefaultPath => Combine(DataDirectory, "Elfar");

        private static string Combine(params string[] paths) => System.IO.Path.Combine(paths);

        private static string Remainder(string value, int index)
        {
            if (value[index] == 92) index++;
            return value.Substring(index);
        }

        private static string Resolve(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.StartsWith("..")) value = DefaultPath;
            else if (value.StartsWith(".")) value = Combine(AppDomain.CurrentDomain.BaseDirectory, Remainder(value, 2));
            else if (value.StartsWith("~/")) value = HostingEnvironment.MapPath(value);
            else if (value.StartsWith(DataDirectoryMacroString)) value = Combine(DataDirectory, Remainder(value, DataDirectoryMacroString.Length));
            // ReSharper disable once PossibleNullReferenceException
            return value.Replace("/", @"\");
        }
    }
}