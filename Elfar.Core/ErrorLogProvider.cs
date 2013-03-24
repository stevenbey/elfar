using System;
using System.Collections.Generic;

namespace Elfar
{
    public abstract class ErrorLogProvider : IErrorLogProvider
    {
        public abstract void Delete(Guid id);
        public abstract ErrorLog Get(Guid id);
        public abstract IList<ErrorLog> List();
        public abstract void Save(ErrorLog errorLog);

        protected virtual void SetConnectionString(string value)
        {
            Settings.ConnectionString = value;
        }
        protected static void TryExecute(Action action)
        {
            try { action(); }
            catch(Exception) { }
        }

        public string Application
        {
            get { return Settings.Application; }
        }
        public static ErrorLogProviderSettings Settings { get; set; }
        
        protected string ConnectionString
        {
            get { return Settings.ConnectionString; }
            set { SetConnectionString(value); }
        }
    }
}