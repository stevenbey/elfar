using System;

namespace Elfar.Data
{
    public class DbErrorLog
    {
        public static explicit operator DbErrorLog(ErrorLog errorLog)
        {
            return new DbErrorLog
            {
                Application = errorLog.Application,
                Code = errorLog.Code,
                Cookies = errorLog.Cookies,
                Detail = errorLog.Detail,
                Form = errorLog.Form,
                Host = errorLog.Host,
                Html = errorLog.Html,
                ID = errorLog.ID,
                Message = errorLog.Message,
                QueryString = errorLog.QueryString,
                ServerVariables = errorLog.ServerVariables,
                Source = errorLog.Source,
                Time = errorLog.Time,
                Type = errorLog.Type,
                User = errorLog.User
            };
        }
        public static implicit operator ErrorLog(DbErrorLog errorLog)
        {
            return new ErrorLog
            {
                Application = errorLog.Application,
                Code = errorLog.Code,
                Cookies = errorLog.Cookies,
                Detail = errorLog.Detail,
                Form = errorLog.Form,
                Host = errorLog.Host,
                Html = errorLog.Html,
                ID = errorLog.ID,
                Message = errorLog.Message,
                QueryString = errorLog.QueryString,
                ServerVariables = errorLog.ServerVariables,
                Source = errorLog.Source,
                Time = errorLog.Time,
                Type = errorLog.Type,
                User = errorLog.User
            };
        }

        public string Application { get; set; }
        public int? Code { get; set; }
        public string Cookies { get; set; }
        public string Detail { get; set; }
        public string Form { get; set; }
        public string Host { get; set; }
        public string Html { get; set; }
        public Guid ID { get; set; }
        public string Message { get; set; }
        public string QueryString { get; set; }
        public string ServerVariables { get; set; }
        public string Source { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
    }
}