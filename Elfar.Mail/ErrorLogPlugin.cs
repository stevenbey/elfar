using System.ComponentModel.Composition;
using System.Net.Mail;
using System.Text;
using Postal;

namespace Elfar.Mail
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
            if(string.IsNullOrWhiteSpace(Settings.To)) return;
            dynamic email = new Email("ErrorLog");
            email.From = Settings.From ?? "elfar@" + errorLog.Host;
            email.To = Settings.To;
            email.Subject = string.Format(Settings.SubjectFormat ?? "Error ({0}): {1}", errorLog.Type, errorLog.Message).Replace(@"\r\n", " ");
            email.Time = errorLog.Time;
            email.Detail = errorLog.Detail;
            email.ServerVariables = errorLog.ServerVariables;
            if(Settings.AttachOriginalError && !string.IsNullOrWhiteSpace(errorLog.Html))
                email.Attach(Attachment.CreateAttachmentFromString(errorLog.Html, "Original ASP.NET error page.html", Encoding.UTF8, "text/html"));
            email.SendAsync();
        }

        public static Settings Settings { get; set; }
    }
}