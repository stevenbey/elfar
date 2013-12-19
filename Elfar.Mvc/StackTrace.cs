using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Elfar.Mvc
{
    public class StackTrace
    {
        public StackTrace(string detail)
        {
            lines = WebUtility.HtmlEncode(detail).Replace("\r", "").Split('\n').Select(StackTraceLineFactory.Create);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, lines);
        }
        
        readonly IEnumerable<StackTraceLine> lines;
    }

    class StackTraceLineFactory
    {
        public static StackTraceLine Create(string line)
        {
            if (line.StartsWith("   at")) return new MethodLine(line);
            return new StackTraceLine(line);
        }
    }

    public class StackTraceLine
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
        public MethodLine(string line) : base(line) {}

        public override string ToString()
        {
            var line = Line;
            line = Regex.Replace(line, @"(?<=(at\s))(([a-zA-Z0-9_`]|&lt;&gt;)*\.)*", m => Span.ToString("type", m.Value));
            line = Regex.Replace(line, @"(?<=(</span>)).*?(?=\()", m => Span.ToString("method", m.Value));
            line = Regex.Replace(line, @"(?<=(\(|,\s)).*?(?=\s)", m => Span.ToString("type", m.Value));
            line = Regex.Replace(line, @"(?<=(</span>\s)).*?(?=(,|\)))", m => Span.ToString("name", m.Value));
            if (line.IndexOf(" in ", StringComparison.Ordinal) > 0)
            {
                line = Regex.Replace(line, @"(?<=(in\s)).*?(?=(\:line))", m => Span.ToString("file", m.Value));
                line = Regex.Replace(line, @"(?<=(\:line\s)).*", m => Span.ToString("line", m.Value));
            }
            return line;
        }
    }

    class Span : TagBuilder
    {
        public Span(string @class = null) : base("span")
        {
            if (@class != null) AddCssClass(@class);
        }
        
        public static string ToString(string @class, string value)
        {
            return new Span(@class) { InnerHtml = value }.ToString();
        }
    }
}