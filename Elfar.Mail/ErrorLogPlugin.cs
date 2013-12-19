using Postal;

namespace Elfar.Mail
{
    public class ErrorLogPlugin : IErrorLogPlugin
    {
        public void Execute(ErrorLog errorLog)
        {
            if(string.IsNullOrWhiteSpace(Settings.To)) return;
            dynamic email = new Email("ErrorLog");
            email.From = Settings.From ?? "elfar@" + errorLog.Host;
            email.To = Settings.To;
            email.Subject = string.Format(Settings.SubjectFormat ?? "Error ({0}): {1}", errorLog.Type, errorLog.Message).Replace(@"\r\n", " ");
            email.Time = errorLog.Time;
            email.Source = errorLog.Source;
            email.StackTrace = errorLog.StackTrace;
            email.SendAsync();
        }
    }
}