using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Elfar.IO
{
    public abstract class FileErrorLogProvider : IErrorLogProvider
    {
        protected FileErrorLogProvider()
        {
            var settings = ErrorLogProvider.Settings as Settings;
            var path = settings == null ? DefaultFilePath : settings.FilePath;
            if(string.IsNullOrWhiteSpace(path)) return;
            if(path.StartsWith("~/")) path = HostingEnvironment.MapPath(path);
            else if(path.StartsWith(".")) path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.Substring(2));
            else if(path.StartsWith(dataDirectoryMacroString))
            {
                var index = dataDirectoryMacroString.Length;
                if(path[index] == 92) index++;
                path = Path.Combine(DataDirectory, path.Substring(index));
            }
            FilePath = path;
        }

        public abstract void Delete(int id);
        public abstract void Save(ErrorLog errorLog);
        
        protected abstract string GetDefaultFilePath();

        public virtual IEnumerable<ErrorLog> All
        {
            get { throw new NotImplementedException(); }
        }

        protected string FilePath { get; private set; }

        string DataDirectory
        {
            get
            {
                var currentDomain = AppDomain.CurrentDomain;
                var dataDirectory = currentDomain.GetData(dataDirectoryMacroString.Trim('|')) as string;
                if(String.IsNullOrEmpty(dataDirectory)) dataDirectory = currentDomain.BaseDirectory;
                return dataDirectory;
            }
        }
        string DefaultFilePath
        {
            get { return GetDefaultFilePath(); }
        }

        protected static readonly object Key = new object();

        const string dataDirectoryMacroString = "|DataDirectory|";
    }
}