using System;
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

            RouteConstraints = 
            RouteDefaults = 
            RouteData = 
            DataTokens = 
            Cookies =
            Form =
            QueryString =
            ServerVariables = Dictionary.Empty;

        }

        public string Action { get; protected set; }
        public string Area { get; protected set; }
        public int? Code { get; protected set; }
        public string Controller { get; protected set; }
        public Dictionary Cookies { get; protected set; }
        public Dictionary DataTokens { get; protected set; }
        public string Date { get; private set; }
        public Dictionary Form { get; protected set; }
        public string Host { get; protected set; }
        public string Html { get; protected set; }
        public string HttpMethod { get; protected set; }
        public Guid ID { get; private set; }
        public string Message { get; private set; }
        public Dictionary QueryString { get; protected set; }
        public Dictionary RouteConstraints { get; protected set; }
        public Dictionary RouteData { get; protected set; }
        public Dictionary RouteDefaults { get; protected set; }
        public string RouteUrl { get; protected set; }
        public Dictionary ServerVariables { get; protected set; }
        public string Source { get; private set; }
        public string StackTrace { get; private set; }
        public string Time { get; private set; }
        public string Type { get; private set; }
        public Uri Url { get; protected set; }
        public string User { get; protected set; }

        public class Data
        {
            public Data() { }

            Data(ErrorLog errorLog)
            {
                ID = errorLog.ID.ToString("N");
                Details = serializer.Serialize(new
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

            public static implicit operator Data(ErrorLog errorLog)
            {
                return new Data(errorLog);
            }

            internal Data Decompress()
            {
                Details = Details.Decompress();
                Summary = Summary.Decompress();
                return this;
            }
            internal Data Compress()
            {
                Details = Details.Compress();
                Summary = Summary.Compress();
                return this;
            }

            public string Details { get; private set; }
            public string ID { get; private set; }
            public string Summary { get; private set; }

            static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        }
    }
}