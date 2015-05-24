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
            File.Delete(GetFilePath(id));
        }

        protected override string Read(string name)
        {
            return File.ReadAllText(GetFilePath(name));
        }
        protected override void Save(string name, string value)
        {
            File.WriteAllText(GetFilePath(name), value);
        }

        static string GetFilePath(string name)
        {
            return System.IO.Path.Combine(Path, name + ".json");
        }
        static bool Init()
        {
            if (Directory.Exists(Path)) return false;
            Directory.CreateDirectory(Path);
            return true;
        }
    }
}