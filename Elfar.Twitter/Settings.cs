using System;

namespace Elfar.Twitter
{
    public static class Settings
    {
        public static string Ellipsis { get; set; }
        public static string FormFormat { get; set; }
        public static int? MaxStatusLength { get; set; }
        public static string Password { get; set; }
        public static Func<ErrorLog, string> Status { get; set; }
        public static string Url { get; set; }
        public static string Username { get; set; }
    }
}