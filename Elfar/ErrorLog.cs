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
            if (exception == null) throw new ArgumentNullException("exception");

            Time = DateTime.Now;

            try { Host = Environment.MachineName; }
            catch (SecurityException) { }

            var @base = exception.GetBaseException();

            Message = @base.Message;
            Source = @base.Source;
            StackTrace = new StackTraceBuilder(@base).ToString();
            Type = @base.GetType().ToString();

            User = Thread.CurrentPrincipal.Identity.Name;

            ID = Math.Abs((HttpRuntime.AppDomainAppId + Host + Type + Time + User).GetHashCode() + @base.GetHashCode());
        }

        public string Host { get; protected set; }
        public int ID { get; private set; }
        public string Message { get; private set; }
        public string Source { get; private set; }
        public string StackTrace { get; set; }
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

            public static implicit operator Storage(ErrorLog errorLog)
            {
                return new Storage(errorLog);
            }

            public int ID { get; protected set; }
            public string Json { get; protected set; }
            
            static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
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