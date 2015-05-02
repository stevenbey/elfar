namespace Elfar.IO
{
    public abstract class FileErrorLogProvider : IErrorLogProvider
    {
        static FileErrorLogProvider()
        {
            path = (ErrorLogProvider.Settings as Settings ?? new Settings()).Path;
        }

        public abstract void Delete(string id);
        public abstract void Save(ErrorLog.Storage errorLog);

        public abstract string Summaries { get; set; }
        public abstract string this[string id] { get; set; }

        protected string Path
        {
            get { return path; }
        }

        static readonly string path;
    }
}