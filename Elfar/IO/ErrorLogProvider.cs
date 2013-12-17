using System.Collections.Generic;
using System.Web.Hosting;

namespace Elfar.IO
{
    public abstract class ErrorLogProvider : IErrorLogProvider
    {
        protected ErrorLogProvider()
        {
            var settings = Elfar.ErrorLogProvider.Settings as Settings;
            var path = settings == null ? DefaultFilePath : settings.FilePath;
            if(!string.IsNullOrWhiteSpace(path) && path.StartsWith("~/")) path = HostingEnvironment.MapPath(path);
            FilePath = path;
        }

        public abstract void Delete(int id);
        public abstract void Save(ErrorLog errorLog);
        
        protected abstract string GetDefaultFilePath();

        public abstract IEnumerable<ErrorLog> All { get; }

        protected string FilePath { get; private set; }

        string DefaultFilePath
        {
            get { return GetDefaultFilePath(); }
        }

        protected static readonly object Key = new object();
    }
}