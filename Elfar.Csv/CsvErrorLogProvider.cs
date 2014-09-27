using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Elfar.IO;

namespace Elfar.Csv
{
    [DisplayName("CSV")]
    public sealed class CsvErrorLogProvider : FileErrorLogProvider, IStorageProvider
    {
        public CsvErrorLogProvider()
        {
            if (!File.Exists(FilePath)) File.WriteAllText(FilePath, columns);
        }
        
        public override void Delete(int id)
        {
            File.WriteAllLines(FilePath, Remove(id.ToString(CultureInfo.InvariantCulture)));
        }
        public new IEnumerator<ErrorLog.Storage> GetEnumerator()
        {
            return File.ReadLines(FilePath).Skip(1).Select(s => new errorLog(s)).GetEnumerator();
        }
        public override void Save(ErrorLog errorLog)
        {
            File.AppendAllLines(FilePath, new[] { new errorLog(errorLog).ToString() });
        }

        protected override string GetDefaultFilePath()
        {
            return defaultFilePath;
        }

        IEnumerable<string> Remove(string id)
        {
            id += ",";
            return File.ReadAllLines(FilePath).Where(l => !l.StartsWith(id));
        }

        const string columns = "ID,Value";
        const string defaultFilePath = "|DataDirectory|Elfar.csv";

        // ReSharper disable once InconsistentNaming
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