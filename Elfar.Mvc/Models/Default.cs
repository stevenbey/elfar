namespace Elfar.Mvc.Models
{
    public class Default
    {
        public Elfar.ErrorLog ErrorLog { get; set; }
        public StackTrace StackTrace
        {
            get
            {
                var detail = ErrorLog.Detail;
                return string.IsNullOrWhiteSpace(detail) ? null : new StackTrace(detail);
            }
        }
    }
}