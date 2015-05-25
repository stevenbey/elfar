using System;
using System.Collections.Generic;
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
            Summaries = Regex.Replace(Regex.Replace(Summaries, string.Format(format, id), ""), @"\[,|,,|,]", m => replacements[m.Value]);
        }
        public void Save(ErrorLog.Storage errorLog)
        {
            Summaries = string.Concat("[", string.Join(",", new List<string>(Summaries.Trim('[', ']').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) { errorLog.Summary }), "]");
            this[errorLog.ID] = errorLog.Detail;
        }

        protected abstract string Read(string fileName);
        protected abstract void Write(string fileName, string value);

        public string Summaries
        {
            get { return Read(summaries); }
            set { Write(summaries, value); }
        }
        public string this[string id]
        {
            get { return Read(id + Ext); }
            set { Write(id + Ext, value); }
        }

        protected const string Ext = ".json";

        protected static readonly string Path;

        const string format = @"\{{.*?""ID"":""{0}"".*?}}";
        const string summaries = "summaries" + Ext;
        
        static readonly Dictionary replacements = new Dictionary { { "[,", "[" }, { ",,", "," }, { ",]", "]" } };
    }
}