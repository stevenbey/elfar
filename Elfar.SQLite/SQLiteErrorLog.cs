using System;

namespace Elfar.SQLite
{
    class SQLiteErrorLog
    {
        public static implicit operator ErrorLog(SQLiteErrorLog errorLog)
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
                    ID = new Guid(errorLog.ID),
                    Message = errorLog.Message,
                    QueryString = errorLog.QueryString,
                    ServerVariables = errorLog.ServerVariables,
                    Source = errorLog.Source,
                    Time = DateTime.Parse(errorLog.Time),
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
        public byte[] ID { get; set; }
        public string Message { get; set; }
        public string QueryString { get; set; }
        public string ServerVariables { get; set; }
        public string Source { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
    }
}