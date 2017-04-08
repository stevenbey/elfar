namespace Elfar.Mail
{
    public class Settings : Elfar.Settings
    {
        bool? attachOriginalError;
        string to, from, subjectFormat;

        public bool AttachOriginalError
        {
            get
            {
                if(!this.attachOriginalError.HasValue)
                {
                    bool setting;
                    var value = this["Mail.AttachOriginalError"];
                    this.attachOriginalError = !string.IsNullOrWhiteSpace(value) && bool.TryParse(value, out setting) && setting;
                }

                return (bool)this.attachOriginalError;
            }
        }
        public string From => this.@from ?? (this.@from = this["Mail.From"]);

        public string SubjectFormat => this.subjectFormat ?? (this.subjectFormat = this["Mail.SubjectFormat"]);

        public string To => this.to ?? (this.to = this["Mail.To"]);
    }
}