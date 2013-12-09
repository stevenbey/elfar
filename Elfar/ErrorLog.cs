using System;
using System.Security;
using System.Threading;
using System.Web;

namespace Elfar
{
    [Serializable]
    public class ErrorLog
    {
        public ErrorLog() {}
        public ErrorLog(string application, Exception exception) : this(application, (Json) exception) {}
        public ErrorLog(string application, Json json)
        {
            Application = application;
            Json = json;
        }

        public string Application { get; set; }
        public int ID { get; set; }
        public Json Json { get; set; }
    }
}