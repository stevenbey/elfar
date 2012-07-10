using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Xml.Serialization;
using Ionic.Zip;
using Ionic.Zlib;

namespace Elfar.Zip
{
    public class ZipErrorLogProvider
        : IErrorLogProvider
    {
        public ZipErrorLogProvider(
            string application = null,
            string path = @default)
        {
            Application = string.IsNullOrWhiteSpace(application) ? null : application;
            if(string.IsNullOrWhiteSpace(path)) path = @default;
            if(path.StartsWith("~/")) path = HostingEnvironment.MapPath(path);
            this.path = path;
            var file = new FileInfo(path);
            if(file.Exists) return;
            lock(key)
            {
                if(file.Exists) return;
                try { using(file.Create()) {} }
                catch(Exception) {}
            }
        }

        public void Delete(Guid id)
        {
            using(var zip = new ZipFile(path))
            {
                zip.RemoveEntry(id + ".xml");
                zip.Save();
            }
        }
        public ErrorLog Get(Guid id)
        {
            using(var zip = new ZipFile(path)) return ErrorLog(zip.SingleOrDefault(e => e.FileName == id + ".xml"));
        }
        public IList<ErrorLog> List()
        {
            using(var zip = new ZipFile(path))
                return new List<ErrorLog>(zip.Select(ErrorLog).OrderByDescending(e => e.Time));
        }
        public void Save(ErrorLog errorLog)
        {
            try
            {
                Save(errorLog, new ZipFile(path), zip => zip.Save());
            }
            catch(Exception)
            {
                Save(errorLog, new ZipFile { CompressionLevel = CompressionLevel.BestCompression }, zip => zip.Save(path));
            }
        }
        
        static ErrorLog ErrorLog(ZipEntry entry)
        {
            using(var reader = entry.OpenReader())
            using(var stream = new StreamReader(reader))
                return (ErrorLog) serializer.Deserialize(stream);
        }
        static void Save(ErrorLog errorLog, ZipFile zip, Action<ZipFile> action)
        {
            using(zip)
            {
                var builder = new StringBuilder();
                using(var stream = new StringWriter(builder)) serializer.Serialize(stream, errorLog);
                zip.AddEntry(errorLog.ID + ".xml", builder.ToString());
                action(zip);
            }
        }

        public string Application { get; private set; }

        readonly string path;

        const string @default = "~/App_Data/Errors.zip";

        static readonly object key = new object();
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ErrorLog));
    }
}