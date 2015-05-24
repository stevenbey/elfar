using System;
using System.Text.RegularExpressions;

namespace Elfar.IO
{
    public abstract class FileErrorLogProvider : IErrorLogProvider
    {
        static FileErrorLogProvider()
        {
            Path = (ErrorLogProvider.Settings as Settings ?? new Settings()).Path;
        }

        protected FileErrorLogProvider(Func<bool> init)
        {
            if(init()) Summaries = "[]";
        }

        public virtual void Delete(string id)
        {
            Summaries = Regex.Replace(Summaries, string.Format(format, id), ",").Replace(",,", ",");
        }
        public void Save(ErrorLog.Storage errorLog)
        {
            Summaries = Summaries.Insert(Summaries.Length - 1, "," + errorLog.Summary).Replace("[,", "[");
            this[errorLog.ID] = errorLog.Detail;
        }

        protected abstract string Read(string name);
        protected abstract void Save(string name, string value);

        public string Summaries
        {
            get { return Read("summaries"); }
            set { Save("summaries", value); }
        }
        public string this[string id]
        {
            get { return Read(id); }
            set { Save(id, value); }
        }

        protected static readonly string Path;
        
        const string format = @",?\{{.*?""ID"":""{0}"".*?}},?";
    }
}