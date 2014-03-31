using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text.RegularExpressions;
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

            Time = DateTime.Now;

            try { Host = Environment.MachineName; }
            catch(SecurityException) { }

            var @base = exception.GetBaseException();

            Message = @base.Message;
            Source = @base.Source;
            StackTrace = new StackTraceBuilder(@base).ToString();
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
        public string StackTrace { get; set; }
        public DateTime Time { get; private set; }
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
            public string Json
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

        class StackTraceBuilder
        {
            public StackTraceBuilder(Exception exception)
            {
                lines = WebUtility.HtmlEncode(exception.ToString()).Replace("\r", "").Split('\n').Select(CreateLine);
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, lines);
            }

            static StackTraceLine CreateLine(string line)
            {
                if(line.StartsWith("   at")) return new MethodLine(line);
                return new StackTraceLine(line);
            }

            readonly IEnumerable<StackTraceLine> lines;

            class StackTraceLine
            {
                public StackTraceLine(string line)
                {
                    Line = line;
                }

                public override string ToString()
                {
                    return Line;
                }

                protected string Line { get; private set; }
            }

            class MethodLine : StackTraceLine
            {
                public MethodLine(string line) : base(line) { }

                public override string ToString()
                {
                    var line = Line;
                    line = Regex.Replace(line, @"(?<=(at\s))(([a-zA-Z0-9_`]|&lt;&gt;)*\.)*", m => Span("type", m.Value));
                    line = Regex.Replace(line, @"(?<=(</span>)).*?(?=\()", m => Span("method", m.Value));
                    line = Regex.Replace(line, @"(?<=(\(|,\s)).*?(?=\s)", m => Span("type", m.Value));
                    line = Regex.Replace(line, @"(?<=(</span>\s)).*?(?=(,|\)))", m => Span("name", m.Value));
                    if(line.IndexOf(" in ", StringComparison.Ordinal) > 0)
                    {
                        line = Regex.Replace(line, @"(?<=(in\s)).*?(?=(\:line))", m => Span("file", m.Value));
                        line = Regex.Replace(line, @"(?<=(\:line\s)).*", m => Span("line", m.Value));
                    }
                    return line;
                }

                static string Span(string @class, string value)
                {
                    return string.Format(@"<span class=""{0}"">{1}</span>", @class, value);
                }
            }
        }
    }
}