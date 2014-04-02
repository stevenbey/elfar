namespace Elfar.Mvc.ActionResults
{
    internal class DashboardResult : ErrorLogResult
    {
        protected override string Content
        {
            get { return "[" + string.Join(",", ErrorLogProvider.Summaries) + "]"; }
        }
    }
}