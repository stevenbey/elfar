using System.Collections.Generic;
using System.Linq;
using Simple.Data;

namespace Elfar.Data
{
    public class ErrorLogProvider : IErrorLogProvider, IJsonProvider
    {
        static ErrorLogProvider()
        {
            settings = Elfar.ErrorLogProvider.Settings as Settings ?? new Settings();
            var obj = Database.OpenConnection(settings.ConnectionString);
            if(!string.IsNullOrWhiteSpace(settings.Schema)) obj = obj[settings.Schema];
            if(string.IsNullOrWhiteSpace(settings.Table)) settings.Table = "Elfar_ErrorLogs";
            errorLogs = obj[settings.Table];
        }

        public void Delete(int id)
        {
            errorLogs.Delete(ID: id);
        }
        public void Save(ErrorLog errorLog)
        {
            errorLogs.Insert(new errorLog(errorLog));
        }

        IEnumerable<ErrorLog> IErrorLogProvider.All
        {
            get { throw new System.NotImplementedException(); }
        }
        IEnumerable<string> IJsonProvider.Json
        {
            get { return ((IEnumerable<string>) errorLogs.All().Select(errorLogs.ErrorLog)).Select(s => s.Decompress()); }
        }

        static readonly Settings settings;
        static readonly dynamic errorLogs;

        class errorLog : ErrorLog.Storage
        {
            public errorLog(ErrorLog errorLog) : base(errorLog) {}

            public string ErrorLog
            {
                get { return Json.Compress(); }
                set { Json = value.Decompress(); }
            }

            new string Json
            {
                get { return base.Json; }
                set { base.Json = value; }
            }
        }
    }
}