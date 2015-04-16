using System;
using System.IO;
using System.Text;
using Elfar.IO;

namespace Elfar.Json
{
    public class JsonErrorLogProvider : FileErrorLogProvider
    {
        public JsonErrorLogProvider()
        {
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
        }

        public override void Delete(Guid id)
        {
            File.Delete(GetFilePath(id));
        }
        public override void Save(ErrorLog.Storage errorLog)
        {
            var summaries = new StringBuilder(Summaries);
            var length = Summaries.Length;
            summaries.Insert(length - 1, (length > 2 ? "," : null) + errorLog.Summary);
            Summaries = summaries.ToString();
            this[errorLog.ID] = errorLog.Detail;
        }

        string GetFilePath<T>(T name)
        {
            return System.IO.Path.Combine(Path, name + ".json");
        }

        public override string Summaries
        {
            get
            {
                var filePath = GetFilePath("summaries");
                return File.Exists(filePath) ? File.ReadAllText(filePath) : "[]";
            }
            set { File.WriteAllText(GetFilePath("summaries"), value); }
        }
        public override string this[Guid id]
        {
            get { return this[id.ToString()]; }
            set { this[id.ToString()] = value; }
        }

        string this[string id]
        {
            get { return File.ReadAllText(GetFilePath(id)); }
            set { File.WriteAllText(GetFilePath(id), value); }
        }
    }
}