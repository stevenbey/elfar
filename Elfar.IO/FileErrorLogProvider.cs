using System;
using System.Collections;
using System.Collections.Generic;

namespace Elfar.IO
{
    public abstract class FileErrorLogProvider : IErrorLogProvider
    {
        protected FileErrorLogProvider()
        {
            if (Settings.FilePath == null) Settings.FilePath = DefaultFilePath;
        }

        public void Add(object obj) { }
        public abstract void Delete(int id);
        public virtual IEnumerator<ErrorLog> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        public abstract void Save(ErrorLog errorLog);

        protected abstract string GetDefaultFilePath();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ErrorLog>) this).GetEnumerator();
        }

        protected static string FilePath
        {
            get { return Settings.FilePath; }
        }
        
        string DefaultFilePath
        {
            get { return GetDefaultFilePath(); }
        }
        static Settings Settings
        {
            get { return settings ?? (settings = new Settings()); }
        }

        protected static readonly object Key = new object();

        static Settings settings;
    }
}