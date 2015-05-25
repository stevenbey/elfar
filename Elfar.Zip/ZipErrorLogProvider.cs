using System.IO;
using System.IO.Compression;
using Elfar.IO;

namespace Elfar.Zip
{
    public class ZipErrorLogProvider : FileErrorLogProvider
    {
        public ZipErrorLogProvider() : base(Init) { }

        public override void Delete(string id)
        {
            base.Delete(id);
            using (var archive = Archive)
            {
                var entry = archive.GetEntry(id + Ext);
                if (entry == null) return;
                lock (key) entry.Delete();
            }
        }

        protected override string Read(string fileName)
        {
            using (var archive = Archive)
            {
                var entry = archive.GetEntry(fileName);
                if (entry == null) return null;
                using (var reader = new StreamReader(entry.Open())) return reader.ReadToEnd();
            }
        }
        protected override void Write(string fileName, string value)
        {
            using (var archive = Archive)
            using (var writer = new StreamWriter((archive.GetEntry(fileName) ?? archive.CreateEntry(fileName)).Open()))
            lock (key) writer.Write(value);
        }
        
        static bool Init()
        {
            return !File.Exists(Path);
        }

        static ZipArchive Archive
        {
            get { return ZipFile.Open(Path, ZipArchiveMode.Update); }
        }

        static readonly object key = new object();
    }
}