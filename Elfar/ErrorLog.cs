using System;
using System.Security;
using System.Threading;
using System.Web.Script.Serialization;

namespace Elfar
{
    public class ErrorLog
    {
        public ErrorLog() { }

        internal ErrorLog(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            Time = DateTime.Now;

            try { Host = Environment.MachineName; }
            catch (SecurityException) { }

            var @base = exception.GetBaseException();

            Message = @base.Message;
            Source = @base.Source;
            StackTrace = @base.ToString();
            Type = @base.GetType().ToString();

            User = Thread.CurrentPrincipal.Identity.Name;

            ID = Math.Abs((Application + Host + Type + Time + User).GetHashCode() + @base.GetHashCode());
        }

        public string Application
        {
            get { return ErrorLogProvider.Settings.Application; }
        }
        public string Host { get; protected set; }
        public int ID { get; private set; }
        public string Message { get; private set; }
        public string Source { get; private set; }
        public string StackTrace { get; protected set; }
        public DateTime Time { get; private set; }
        public string Type { get; private set; }
        public string User { get; protected set; }

        public class Storage
        {
            internal Storage() { }
            internal Storage(ErrorLog errorLog)
            {
                ID = errorLog.ID;
                Json = serializer.Serialize(errorLog);
            }

            public int ID { get; set; }
            public string Json { get; set; }
            
            static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        }
    }
}