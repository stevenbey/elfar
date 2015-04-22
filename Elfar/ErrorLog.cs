using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Web.Script.Serialization;

namespace Elfar
{
    public class ErrorLog
    {
        internal ErrorLog(Exception exception)
        {
            if(exception == null) throw new ArgumentNullException("exception");

            ID = Guid.NewGuid();

            var now = DateTime.Now;
            Date = now.ToString("yyyy-MM-dd");
            Time = now.ToShortTimeString();

            try { Host = Environment.MachineName; }
            catch(SecurityException) { }

            var @base = exception.GetBaseException();

            Message = @base.Message;
            Source = @base.Source;
            StackTrace = @base.ToString().Replace(Environment.NewLine, "\n");
            Type = @base.GetType().ToString();

            User = Thread.CurrentPrincipal.Identity.Name;
        }

        public string Action { get; set; }
        public string Area { get; set; }
        public int? Code { get; set; }
        public string Controller { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public Dictionary<string, object> DataTokens { get; set; }
        public string Date { get; private set; }
        public Dictionary<string, string> Form { get; set; }
        public string Host { get; protected set; }
        public string Html { get; set; }
        public string HttpMethod { get; set; }
        public Guid ID { get; private set; }
        public string Message { get; private set; }
        public Dictionary<string, string> QueryString { get; set; }
        public Dictionary<string, object> RouteConstraints { get; set; }
        public Dictionary<string, object> RouteData { get; set; }
        public Dictionary<string, object> RouteDefaults { get; set; }
        public string RouteUrl { get; set; }
        public Dictionary<string, string> ServerVariables { get; set; }
        public string Source { get; private set; }
        public string StackTrace { get; private set; }
        public string Time { get; private set; }
        public string Type { get; private set; }
        public Uri Url { get; set; }
        public string User { get; protected set; }

        public class Storage
        {
            public Storage() { }
            
            Storage(ErrorLog errorLog)
            {
                ID = errorLog.ID.ToString("N");
                Detail = serializer.Serialize(new
                {
                    errorLog.Code,
                    errorLog.Cookies,
                    errorLog.DataTokens,
                    errorLog.Form,
                    errorLog.Host,
                    errorLog.Html,
                    errorLog.Message,
                    errorLog.QueryString,
                    errorLog.RouteConstraints,
                    errorLog.RouteData,
                    errorLog.RouteDefaults,
                    errorLog.RouteUrl,
                    errorLog.ServerVariables,
                    errorLog.Source,
                    errorLog.StackTrace,
                    errorLog.Url,
                    errorLog.User
                });
                Summary = serializer.Serialize(new
                {
                    errorLog.Action,
                    errorLog.Area,
                    errorLog.Controller,
                    errorLog.Date,
                    errorLog.HttpMethod,
                    ID,
                    errorLog.Time,
                    errorLog.Type
                });
            }

            public static implicit operator Storage(ErrorLog errorLog)
            {
                return new Storage(errorLog);
            }

            internal Storage Decompress()
            {
                Detail = Detail.Decompress();
                Summary = Summary.Decompress();
                return this;
            }
            internal Storage Compress()
            {
                Detail = Detail.Compress();
                Summary = Summary.Compress();
                return this;
            }

            public string Detail { get; set; }
            public string ID { get; protected set; }
            public string Summary { get; set; }

            static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        }
    }
}