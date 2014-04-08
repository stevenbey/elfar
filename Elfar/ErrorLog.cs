using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace Elfar
{
    public class ErrorLog
    {
        internal ErrorLog(Exception exception)
        {
            if(exception == null) throw new ArgumentNullException("exception");

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

            ID = Math.Abs((HttpRuntime.AppDomainAppId + Host + Type + Time + User).GetHashCode() + @base.GetHashCode());
        }

        public string Action { get; set; }
        public string Area { get; set; }
        public int? Code { get; set; }
        public string Controller { get; set; }
        public IDictionary<string, string> Cookies { get; set; }
        public IDictionary<string, object> DataTokens { get; set; }
        public string Date { get; private set; }
        public IDictionary<string, string> Form { get; set; }
        public string Host { get; protected set; }
        public string Html { get; set; }
        public string HttpMethod { get; set; }
        public int ID { get; private set; }
        public string Message { get; private set; }
        public IDictionary<string, string> QueryString { get; set; }
        public IDictionary<string, object> RouteData { get; set; }
        public string RouteUrl { get; set; }
        public IDictionary<string, string> ServerVariables { get; set; }
        public string Source { get; private set; }
        public string StackTrace { get; private set; }
        public string Time { get; private set; }
        public string Type { get; private set; }
        public Uri Url { get; set; }
        public string User { get; protected set; }

        public class Storage
        {
            internal Storage() { }
            internal Storage(ErrorLog errorLog)
            {
                ID = errorLog.ID;
                Detail = serializer.Serialize(new
                {
                    errorLog.Code,
                    errorLog.Cookies,
                    errorLog.DataTokens,
                    errorLog.Form,
                    errorLog.Html,
                    errorLog.HttpMethod,
                    errorLog.Message,
                    errorLog.QueryString,
                    errorLog.RouteData,
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
                    errorLog.ID,
                    errorLog.Host,
                    errorLog.Time,
                    errorLog.Type
                });
            }

            public static implicit operator Storage(ErrorLog errorLog)
            {
                return new Storage(errorLog);
            }

            public int ID { get; protected set; }
            public string Value
            {
                get { return string.Concat(Detail, separator, Summary); }
                protected set
                {
                    var parts = value.Split(separator[0]);
                    if(parts.Length < 2) return;
                    Detail = parts[0];
                    Summary = parts[1];
                }
            }
            
            internal string Detail { get; set; }
            internal string Summary { get; set; }

            static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

            const string separator = "¬";
        }
    }
}