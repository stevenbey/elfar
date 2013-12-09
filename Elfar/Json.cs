using System;
using System.Security;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace Elfar
{
    public class Json
    {
        protected Json(Exception exception)
        {
            var @base = exception.GetBaseException();

            ID = int.MaxValue;
            Detail = @base.ToString();
            Host = MachineName;
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

        public static explicit operator Json(Exception exception)
        {
            return new Json(exception);
        }
        public static implicit operator Json(string json)
        {
            return serializer.Deserialize<Json>(json);
        }

        public override string ToString()
        {
            return serializer.Serialize(this);
        }

        public int? Code { get; set; }
        public string Detail { get; set; }
        public string Host { get; set; }
        public string Html { get; set; }
        public int ID { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string User { get; set; }

        static string MachineName
        {
            get
            {
                try { return Environment.MachineName; }
                catch (SecurityException) { }
                return null;
            }
        }

        static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
    }
}