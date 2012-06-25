using System;
using System.Web.Mvc;

namespace Elfar.ActionResults
{
    public class DefaultResult
        : ActionResult
    {
        public DefaultResult(
            Guid id,
            IErrorLogProvider provider,
            Func<ErrorLog, ActionResult> success)
        {
            this.id = id;
            this.provider = provider;
            this.success = success;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            try
            {
                success(provider.Get(id)).ExecuteResult(context);
            }
            catch(Exception e)
            {
                throw new ErrorLogException(e);
            }
        }

        readonly Guid id;
        readonly IErrorLogProvider provider;
        readonly Func<ErrorLog, ActionResult> success;
    }
}