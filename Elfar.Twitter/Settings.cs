using System;

namespace Elfar.Twitter
{
    public class Settings
    {
        public string Ellipsis { get; set; }
        public string FormFormat { get; set; }
        public int? MaxStatusLength { get; set; }
        public string Password { get; set; }
        public Func<ErrorLog, string> Status { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
    }
}