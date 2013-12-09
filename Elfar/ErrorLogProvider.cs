using System.Collections.Generic;

namespace Elfar
{
    public abstract class ErrorLogProvider : IErrorLogProvider
    {
        public abstract void Delete(int id);
        public abstract void Save(ErrorLog errorLog);

        public abstract IEnumerable<ErrorLog> All { get; }
        public string Application
        {
            get { return Settings.Application; }
        }
        
        public static Settings Settings = new Settings();
    }
}