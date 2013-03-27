using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Elfar.Data.Xml
{
    public class XmlErrorLogProvider : FileBasedErrorLogProvider
    {
        public XmlErrorLogProvider()
        {
            directory = new DirectoryInfo(Path);
            if (directory.Exists) return;
            lock (key)
            {
                if (directory.Exists) return;
                TryExecute(() => directory.Create());
            }
        }

        public override void Delete(Guid id)
        {
            File(id).Delete();
            files = null;
        }
        public override ErrorLog Get(Guid id)
        {
            return ErrorLog(File(id));
        }
        public override IList<ErrorLog> List()
        {
            return new List<ErrorLog>(Files.OrderByDescending(f => f.Name, StringComparer.OrdinalIgnoreCase).Select(ErrorLog));
        }
        public override void Save(ErrorLog errorLog)
        {
            using (var writer = File(errorLog).OpenWrite())
            {
                serializer.Serialize(writer, errorLog);
                writer.Flush();
            }
            files = null;
        }

        protected override string GetDefaultPath()
        {
            return @default;
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
            return new FileInfo(System.IO.Path.Combine(directory.FullName, file));
        }
        FileInfo File(Guid id)
        {
            return Files.SingleOrDefault(f => f.Name.Contains(id.ToString()));
        }

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
    }
}