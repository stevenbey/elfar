using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Simple.Data;

namespace Elfar.Data
{
    [DisplayName("Simple.Data")]
    public class ErrorLogProvider : IErrorLogProvider
    {
        static ErrorLogProvider()
        {
            var settings = Elfar.ErrorLogProvider.Settings as Settings ?? new Settings();
            var connectionString = settings.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString)) return;
            var obj = Database.OpenConnection(connectionString);
            if (!string.IsNullOrWhiteSpace(settings.Schema)) obj = obj[settings.Schema];
            errorLogs = obj[settings.Table];
        }

        public void Delete(Guid id)
        {
            errorLogs.Delete(ID: id.ToString());
        }
        public void Save(ErrorLog.Storage errorLog)
        {
            errorLogs.Insert(errorLog.Compress());
        }

        public string Summaries
        {
            get
            {
                List<string> summaries = errorLogs.Select(errorLogs.Summary).ToScalarList<string>();
                return string.Concat("[", string.Join(",", summaries.Select(s => s.Decompress())), "]");
            }
        }
        public string this[Guid id]
        {
            get { return errorLogs.Get(id.ToString()).Detail.Decompress(); }
            set { errorLogs.Update(new {ID = id.ToString(), Detail = value.Compress() }); }
        }

        static readonly dynamic errorLogs;
    }
}