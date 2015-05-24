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
                var entry = archive.GetEntry(id);
                if (entry != null) entry.Delete();
            }
        }

        protected override string Read(string name)
        {
            using (var archive = Archive)
            {
                var entry = archive.GetEntry(name);
                if (entry == null) return null;
                using (var reader = new StreamReader(entry.Open())) return reader.ReadToEnd();
            }
        }
        protected override void Save(string name, string value)
        {
            using (var archive = Archive)
            using (var writer = new StreamWriter((archive.GetEntry(name) ?? archive.CreateEntry(name)).Open())) writer.Write(value);
        }
        
        static bool Init()
        {
            return !File.Exists(Path);
        }

        static ZipArchive Archive
        {
            get { return ZipFile.Open(Path, ZipArchiveMode.Update); }
        }
    }
}