using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Web;

namespace Elfar.Twitter
{
    [Export("Plugin", typeof(IErrorLogPlugin))]
    public class ErrorLogPlugin : IErrorLogPlugin
    {
        public ErrorLogPlugin()
        {
            if(Settings == null) Settings = new Settings();
        }

        public void Execute(ErrorLog errorLog)
        {
            new WebClient
            {
                Credentials = Credentials,
                Headers = new WebHeaderCollection { { "Content-Type", "application/x-www-form-urlencoded" } }
            }.UploadStringAsync(new Uri(Settings.Url ?? "http://twitter.com/statuses/update.xml"), "POST", Format(errorLog));
        }

        static string Format(ErrorLog errorLog)
        {
            var status = (Settings.Status ?? (e => e.Message))(errorLog);
            var maxLength = Settings.MaxStatusLength ?? 140;
            if(status.Length > maxLength)
            {
                var ellipsis = Settings.Ellipsis ?? "\x2026";
                var statusLength = maxLength - ellipsis.Length;
                status = statusLength > -1 ? status.Substring(0, statusLength) + ellipsis : status.Substring(0, maxLength);
            }
            return string.Format(Settings.FormFormat ?? "status={0}", HttpUtility.UrlEncode(status));
        }

        public static Settings Settings { get; set; }
        
        static NetworkCredential Credentials
        {
            get
            {
                return string.IsNullOrWhiteSpace(Settings.Username)
                    || string.IsNullOrWhiteSpace(Settings.Password)
                        ? null
                        : new NetworkCredential(Settings.Username, Settings.Password);
            }
        }
    }
}