using System.Collections.Generic;
using System.IO;
using System.Linq;
using Elfar.IO;

namespace Elfar.Csv
{
    public class CsvErrorLogProvider : FileErrorLogProvider, IJsonProvider
    {
        public CsvErrorLogProvider()
        {
            errorLogs = File.Exists(FilePath) ? File.ReadLines(FilePath).Skip(1).Select(s => new errorLog(s)).ToList() : new List<errorLog>();
        }
        
        public override void Delete(int id)
        {
            var errorLog = errorLogs.FirstOrDefault(l => l.ID == id);
            if(errorLog == null) return;
            errorLogs.Remove(errorLog);
            Save();
        }
        public override void Save(ErrorLog errorLog)
        {
            errorLogs.Add(new errorLog(errorLog));
            Save();
        }

        protected override string GetDefaultFilePath()
        {
            return defaultFilePath;
        }

        void Save()
        {
            File.WriteAllLines(FilePath, new[] { columns }.Concat(errorLogs.Select(l => l.ToString())));
        }

        IEnumerable<string> IJsonProvider.Json
        {
            get { return errorLogs.Select(l => l.Json); }
        }
        
        readonly IList<errorLog> errorLogs;

        const string columns = "ID,Value";
        const string defaultFilePath = "|DataDirectory|Elfar.csv";

        class errorLog : ErrorLog.Storage
        {
            internal errorLog(string value)
            {
                var index = value.IndexOf(',');
                ID = int.Parse(value.Substring(0, index));
                Json = value.Substring(++index).Decompress();
            }
            internal errorLog(ErrorLog errorLog) : base(errorLog) {}

            public override string ToString()
            {
                return string.Format("{0},{1}", ID, Json.Compress());
            }
        }
    }
}