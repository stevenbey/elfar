using System;
using System.Net;
using System.Web;

namespace Elfar.Twitter
{
    public class ErrorLogTweet
        : IErrorLogTweet
    {
        public ErrorLogTweet(
            string username,
            string password)
        {
            Username = username;
            Password = password;
        }

        public void Post(ErrorLog errorLog)
        {
            new WebClient
            {
                Credentials = Credentials,
                Headers = new WebHeaderCollection { { "Content-Type", "application/x-www-form-urlencoded" } }
            }.UploadStringAsync(new Uri(Url ?? "http://twitter.com/statuses/update.xml"), "POST", Format(errorLog));
        }

        string Format(ErrorLog errorLog)
        {
            var status = (Status ?? (e => e.Message))(errorLog);
            var maxLength = MaxStatusLength ?? 140;
            if(status.Length > maxLength)
            {
                var ellipsis = Ellipsis ?? "\x2026";
                var statusLength = maxLength - ellipsis.Length;
                status = statusLength > -1 ? status.Substring(0, statusLength) + ellipsis : status.Substring(0, maxLength);
            }
            return string.Format(FormFormat ?? "status={0}", HttpUtility.UrlEncode(status));
        }

        public string Ellipsis { get; set; }
        public string FormFormat { get; set; }
        public int? MaxStatusLength { get; set; }
        public string Password { get; private set; }
        public Func<ErrorLog, string> Status { get; set; }
        public string Url { get; set; }
        public string Username { get; private set; }

        NetworkCredential Credentials
        {
            get
            {
                return string.IsNullOrWhiteSpace(Username)
                    || string.IsNullOrWhiteSpace(Password)
                        ? null
                        : new NetworkCredential(Username, Password);
            }
        }
    }
}