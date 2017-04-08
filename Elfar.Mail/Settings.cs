namespace Elfar.Mail
{
    public class Settings : Elfar.Settings
    {
        private bool? attachOriginalError;
        private string to, from, subjectFormat;

        public bool AttachOriginalError
        {
            get
            {
                if (this.attachOriginalError.HasValue) return (bool) this.attachOriginalError;

                var value = this[nameof(AttachOriginalError), nameof(Mail)];
                this.attachOriginalError = !string.IsNullOrWhiteSpace(value) && bool.TryParse(value, out bool setting) && setting;

                return (bool)this.attachOriginalError;
            }
        }
        public string From => this.from ?? (this.from = this[nameof(From), nameof(Mail)]);

        public string SubjectFormat => this.subjectFormat ?? (this.subjectFormat = this[nameof(SubjectFormat), nameof(Mail)]);

        public string To => this.to ?? (this.to = this[nameof(To), nameof(Mail)]);
    }
}