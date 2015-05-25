using System.IO;
using Elfar.IO;

namespace Elfar.Json
{
    public class JsonErrorLogProvider : FileErrorLogProvider
    {
        public JsonErrorLogProvider() : base(Init) {}

        public override void Delete(string id)
        {
            base.Delete(id);
            File.Delete(GetFilePath(id + Ext));
        }

        protected override string Read(string fileName)
        {
            return File.ReadAllText(GetFilePath(fileName));
        }
        protected override void Write(string fileName, string value)
        {
            File.WriteAllText(GetFilePath(fileName), value);
        }

        static string GetFilePath(string fileName)
        {
            return System.IO.Path.Combine(Path, fileName);
        }
        static bool Init()
        {
            if (Directory.Exists(Path)) return false;
            Directory.CreateDirectory(Path);
            return true;
        }
    }
}