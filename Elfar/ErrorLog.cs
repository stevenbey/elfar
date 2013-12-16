using System;

namespace Elfar
{
    public class ErrorLog
    {
        public ErrorLog() { }
        
        internal ErrorLog(Exception exception) : this(new Json(exception)) { }
        internal ErrorLog(Json json)
        {
            ID = json.ID;
            Json = json;
        }

        public int ID { get; set; }
        public string Json { get; set; }
    }
}