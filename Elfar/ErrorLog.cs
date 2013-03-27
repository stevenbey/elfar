using System;
using System.Security;
using System.Threading;
using System.Web;

namespace Elfar
{
    [Serializable]
    public class ErrorLog
    {
        public ErrorLog()
        {
            Cookies = new Collection();
            Form = new Collection();
            QueryString = new Collection();
            ServerVariables = new Collection();
        }

        public ErrorLog(string application, Exception exception) : this()
        {
            Application = application;

            var @base = exception.GetBaseException();

            ID = Guid.NewGuid();
            Detail = @base.ToString();
            Host = TryGetMachineName();
            Time = DateTime.Now;

            Message = @base.Message;
            Source = @base.Source;
            Type = @base.GetType().ToString();

            User = Thread.CurrentPrincipal.Identity.Name;

            var httpException = exception as HttpException;

            if (httpException != null)
            {
                Code = httpException.GetHttpCode();
                Html = httpException.GetHtmlErrorMessage();
            }
        }

        static string TryGetMachineName()
        {
            try { return Environment.MachineName; }
            catch (SecurityException) { }
            return null;
        }

        public string Application { get; set; }
        public int? Code { get; set; }
        public Collection Cookies { get; set; }
        public string Detail { get; set; }
        public Collection Form { get; set; }
        public string Host { get; set; }
        public string Html { get; set; }
        public Guid ID { get; set; }
        public string Message { get; set; }
        public Collection QueryString { get; set; }
        public Collection ServerVariables { get; set; }
        public string Source { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
    }
}