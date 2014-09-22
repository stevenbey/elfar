using System.Collections.Generic;
using Simple.Data;

namespace Elfar.Data
{
    public class ErrorLogProvider : IErrorLogProvider, IStorageProvider
    {
        static ErrorLogProvider()
        {
            var settings = Elfar.ErrorLogProvider.Settings as Settings ?? new Settings();
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

        static readonly dynamic errorLogs;

        // ReSharper disable InconsistentNaming
        class errorLog : ErrorLog.Storage
        {
            // ReSharper disable UnusedMember.Local
            public errorLog() { }
            public errorLog(ErrorLog errorLog) : base(errorLog) {}

            public string ErrorLog
            {
                get { return Value.Compress(); }
                set { Value = value.Decompress(); }
            }

            new string Value // Hides the base property
            {
                get { return base.Value; }
                set { base.Value = value; }
            }
        }
    }
}