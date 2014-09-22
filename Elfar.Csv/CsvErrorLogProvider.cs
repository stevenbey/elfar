using System.Collections.Generic;
using System.IO;
using System.Linq;
using Elfar.IO;

namespace Elfar.Csv
{
    // ReSharper disable InconsistentNaming
    public sealed class CsvErrorLogProvider : FileErrorLogProvider, IStorageProvider
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

        IEnumerable<ErrorLog.Storage> IStorageProvider.Items
        {
            get { return errorLogs; }
        }
        
        readonly IList<errorLog> errorLogs;

        const string columns = "ID,Value";
        const string defaultFilePath = "|DataDirectory|Elfar.csv";

        class errorLog : ErrorLog.Storage
        {
            internal errorLog(string value)
            {
                var parts = value.Split(',');
                ID = int.Parse(parts[0]);
                Value = parts[1].Decompress();
            }
            internal errorLog(ErrorLog errorLog) : base(errorLog) {}

            public override string ToString()
            {
                return string.Format("{0},{1}", ID, Value.Compress());
            }
        }
    }
}