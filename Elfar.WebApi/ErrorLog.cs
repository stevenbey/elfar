using System;

namespace Elfar.WebApi
{
    public class ErrorLog
    {
        public static explicit operator ErrorLog(Elfar.ErrorLog errorLog)
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