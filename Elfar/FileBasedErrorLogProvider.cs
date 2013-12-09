using System;
using System.Web.Hosting;
using System.Xml.Serialization;

namespace Elfar
{
    public abstract class FileBasedErrorLogProvider : ErrorLogProvider
    {
        protected FileBasedErrorLogProvider()
        {
            var path = Settings.Path;
            if (String.IsNullOrWhiteSpace(path) && !String.IsNullOrWhiteSpace(DefaultPath)) path = DefaultPath;
            if (path.StartsWith("~/")) path = HostingEnvironment.MapPath(path);
            Path = path;
        }

        protected abstract string GetDefaultPath();

        protected string Path { get; private set; }

        string DefaultPath
        {
            get { return GetDefaultPath(); }
        }

        protected static readonly object key = new object();
        protected static readonly XmlSerializer serializer = new XmlSerializer(typeof(ErrorLog));
    }
}