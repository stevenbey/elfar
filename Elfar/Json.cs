using System;
using System.Security;
using System.Threading;
using System.Web.Script.Serialization;

namespace Elfar
{
    public class Json
    {
        protected Json(Exception exception)
        {
            if(exception == null) throw new ArgumentNullException("exception");

            Time = DateTime.Now;

            try { Host = Environment.MachineName; }
            catch(SecurityException) { }

            var @base = exception.GetBaseException();

            Message = @base.Message;
            Source = @base.Source;
            StackTrace = @base.ToString();
            Type = @base.GetType().ToString();

            User = Thread.CurrentPrincipal.Identity.Name;

            ID = Math.Abs((Host + Type + Time + User).GetHashCode() + exception.GetHashCode() + @base.GetHashCode());
        }

        public static explicit operator Json(Exception exception)
        {
            return new Json(exception);
        }
        public static implicit operator Json(string json)
        {
            return Serializer.Deserialize<Json>(json);
        }
        public static implicit operator string(Json json)
        {
            return json.ToString();
        }

        public override string ToString()
        {
            return Serializer.Serialize(this);
        }

        public string Host { get; protected set; }
        public int ID { get; private set; }
        public string Message { get; private set; }
        public string Source { get; private set; }
        public string StackTrace { get; private set; }
        public DateTime Time { get; private set; }
        public string Type { get; private set; }
        public string User { get; protected set; }
        
        protected static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();
    }
}