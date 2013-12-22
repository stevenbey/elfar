using Postal;

namespace Elfar.Mail
{
    public class ErrorLogPlugin : IErrorLogPlugin
    {
        public void Execute(ErrorLog errorLog)
        {
            if(string.IsNullOrWhiteSpace(Settings.To)) return;
            dynamic email = new Email("ErrorLog");
            email.To = Settings.To;
            email.From = Settings.From ?? "elfar@" + errorLog.Host;
            email.Subject = string.Format(Settings.SubjectFormat ?? "Error Logging Filter and Route (ELFAR): New error logged.");
            email.Time = errorLog.Time;
            email.Source = errorLog.Source;
            email.StackTrace = errorLog.StackTrace;
            email.SendAsync();
        }

        public static Settings Settings
        {
            get { return settings ?? (settings = new Settings()); }
            set { settings = value; }
        }
        
        static Settings settings;
    }
}