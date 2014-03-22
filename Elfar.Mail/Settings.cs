namespace Elfar.Mail
{
    public class Settings : Elfar.Settings
    {
        public bool AttachOriginalError
        {
            get
            {
                if(!attachOriginalError.HasValue)
                {
                    bool setting;
                    var value = GetAppSetting("Mail.AttachOriginalError");
                    attachOriginalError = !string.IsNullOrWhiteSpace(value) && bool.TryParse(value, out setting) && setting;
                }
                return (bool) attachOriginalError;
            }
        }
        public string From
        {
            get { return from ?? (from = GetAppSetting("Mail.From")); }
        }
        public string SubjectFormat
        {
            get { return subjectFormat ?? (subjectFormat = GetAppSetting("Mail.SubjectFormat")); }
        }
        public string To
        {
            get { return to ?? (to = GetAppSetting("Mail.To")); }
        }

        bool? attachOriginalError;
        string from, subjectFormat, to;
    }
}