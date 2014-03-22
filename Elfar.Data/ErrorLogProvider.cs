using System.Collections.Generic;
using Simple.Data;

namespace Elfar.Data
{
    public class ErrorLogProvider : IErrorLogProvider, IStorageProvider
    {
        static ErrorLogProvider()
        {
            settings = Elfar.ErrorLogProvider.Settings as Settings ?? new Settings();
            var obj = Database.OpenConnection(settings.ConnectionString);
            if(!string.IsNullOrWhiteSpace(settings.Schema)) obj = obj[settings.Schema];
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
        IEnumerable<ErrorLog.Storage> IStorageProvider.Items
        {
            get { return ((errorLog[]) errorLogs.All().ToArray<errorLog>()); }
        }

        static readonly Settings settings;
        static readonly dynamic errorLogs;

        class errorLog : ErrorLog.Storage
        {
            public errorLog() {}
            public errorLog(ErrorLog errorLog) : base(errorLog) {}

            public string ErrorLog
            {
                get { return Json.Compress(); }
                set { Json = value.Decompress(); }
            }

            new string Json // Hides the base property
            {
                get { return base.Json; }
                set { base.Json = value; }
            }
        }
    }
}