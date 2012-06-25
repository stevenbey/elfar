using System.Net.Mail;
using System.Text;
using Postal;

namespace Elfar.Mail
{
    public class ErrorLogMail
        : IErrorLogMail
    {
        public ErrorLogMail(
            string to,
            string from = null)
        {
            To = to;
            From = from;
        }

        public void Send(ErrorLog errorLog)
        {
            if(string.IsNullOrWhiteSpace(To)) return;
            dynamic email = new Email("ErrorLog");
            email.From = From ?? "elfar@" + errorLog.Host;
            email.To = To;
            email.Subject = string.Format(SubjectFormat ?? "Error ({0}): {1}", errorLog.Type, errorLog.Message).Replace(@"\r\n", " ");
            email.Time = errorLog.Time;
            email.Detail = errorLog.Detail;
            email.ServerVariables = errorLog.ServerVariables;
            if(AttachOriginalError && !string.IsNullOrWhiteSpace(errorLog.Html))
                email.Attach(Attachment.CreateAttachmentFromString(errorLog.Html, "Original ASP.NET error page.html", Encoding.UTF8, "text/html"));
            email.SendAsync();
        }

        public bool AttachOriginalError { get; set; }
        public string From { get; private set; }
        public string SubjectFormat { get; set; }
        public string To { get; private set; }
    }
}