using System;

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
            ID = json.ID;
            Json = json;
        }

        public string Application { get; set; }
        public int ID { get; set; }
        public Json Json { get; set; }
    }
}