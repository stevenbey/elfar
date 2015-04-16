using System;
using System.ComponentModel;
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
            errorLogs.Insert(errorLog);
        }

        public string Summaries
        {
            get { return string.Concat("[", string.Join(",", errorLogs.Select(errorLogs.Summary).ToScalarList<string>()), "]"); }
        }
        public string this[Guid id]
        {
            get { return errorLogs.Get(id.ToString()).Detail; }
            set { errorLogs.Update(new {ID = id.ToString(), Detail = value}); }
        }

        static readonly dynamic errorLogs;
    }
}