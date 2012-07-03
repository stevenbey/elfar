using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Serialization;

namespace Elfar.Xml
{
    public class XmlErrorLogProvider
        : IErrorLogProvider
    {
        public XmlErrorLogProvider(
            string application = null,
            string path = @default)
        {
            Application = string.IsNullOrWhiteSpace(application) ? null : application;
            if (string.IsNullOrWhiteSpace(path)) path = @default;
            if (path.StartsWith("~/")) path = HostingEnvironment.MapPath(path);
            directory = new DirectoryInfo(path);
            if (directory.Exists) return;
            lock (key)
            {
                if (directory.Exists) return;
                try { directory.Create(); }
                catch(Exception) {}
            }
        }

        public void Delete(Guid id)
        {
            File(id).Delete();
            files = null;
        }
        public ErrorLog Get(Guid id)
        {
            return ErrorLog(File(id));
        }
        public IList<ErrorLog> List()
        {
            return new List<ErrorLog>(Files.OrderByDescending(f => f.Name, StringComparer.OrdinalIgnoreCase).Select(ErrorLog));
        }
        public void Save(ErrorLog errorLog)
        {
            using (var writer = File(errorLog).OpenWrite())
            {
                serializer.Serialize(writer, errorLog);
                writer.Flush();
            }
            files = null;
        }
        
        static ErrorLog ErrorLog(FileInfo file)
        {
            if (file == null) return null;
            using(var reader = file.OpenRead()) return (ErrorLog) serializer.Deserialize(reader);
        }
        FileInfo File(ErrorLog errorLog)
        {
            var file = string.Format(@"ErrorLog-{0:yyyy-MM-ddHHmmssZ}-{1}.xml", errorLog.Time, errorLog.ID);
            if (Application != null) file = Application + "-" + file;
            return new FileInfo(Path.Combine(directory.FullName, file));
        }
        FileInfo File(Guid id)
        {
            return Files.SingleOrDefault(f => f.Name.Contains(id.ToString()));
        }

        public string Application { get; private set; }

        IEnumerable<FileInfo> Files
        {
            get
            {
                var pattern = "ErrorLog-*.xml";
                if (Application != null) pattern = Application + "-" + pattern;
                return files ?? (files = directory.GetFiles(pattern));
            }
        }

        readonly DirectoryInfo directory;
        IEnumerable<FileInfo> files;

        const string @default = "~/App_Data/Errors";

        static readonly object key = new object();
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ErrorLog));
    }
}