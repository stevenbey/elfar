using System.Web.Mvc;

namespace Elfar.ActionResults
{
    public class RssResult
        : IndexResult
    {
        public RssResult(IErrorLogProvider provider) : base(provider) {}

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/xml";
            base.ExecuteResult(context);
        }
    }
}