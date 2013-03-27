using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Ionic.Zlib;

namespace Elfar.Data.Zip
{
    public class ZipErrorLogProvider : FileBasedErrorLogProvider
    {
        public ZipErrorLogProvider()
        {
            var file = new FileInfo(Path);
            if(file.Exists) return;
            lock(key)
            {
                if(file.Exists) return;
                TryExecute(() => { using(file.Create()) {} });
            }
        }

        public override void Delete(Guid id)
        {
            using(var zip = new ZipFile(Path))
            {
                zip.RemoveEntry(id + ".xml");
                zip.Save();
            }
        }
        public override ErrorLog Get(Guid id)
        {
            using(var zip = new ZipFile(Path)) return ErrorLog(zip.SingleOrDefault(e => e.FileName == id + ".xml"));
        }
        public override IList<ErrorLog> List()
        {
            try
            {
                using(var zip = new ZipFile(Path))
                    return new List<ErrorLog>(zip.Select(ErrorLog).OrderByDescending(e => e.Time));
            }
            catch(Exception)
            {
                return new List<ErrorLog>();
            }
        }
        public override void Save(ErrorLog errorLog)
        {
            try
            {
                Save(errorLog, new ZipFile(Path), zip => zip.Save());
            }
            catch(Exception)
            {
                Save(errorLog, new ZipFile { CompressionLevel = CompressionLevel.BestCompression }, zip => zip.Save(Path));
            }
        }

        protected override string GetDefaultPath()
        {
            return @default;
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

        const string @default = "~/App_Data/Errors.zip";
    }
}