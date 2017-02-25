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
                    var setting = false;
                    var value = this["Mail.AttachOriginalError"];
                    attachOriginalError = !string.IsNullOrWhiteSpace(value) && bool.TryParse(value, out setting) && setting;
                }
                return (bool) attachOriginalError;
            }
        }
        public string From
        {
            get { return from ?? (from = this["Mail.From"]); }
        }
        public string SubjectFormat
        {
            get { return subjectFormat ?? (subjectFormat = this["Mail.SubjectFormat"]); }
        }
        public string To
        {
            get { return to ?? (to = this["Mail.To"]); }
        }

        bool? attachOriginalError;
        string from, subjectFormat, to;
    }
}