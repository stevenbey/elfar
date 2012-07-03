using System;
using System.Web.Mvc;

namespace Elfar.ActionResults
{
    public class DeleteResult
        : IndexResult
    {
        public DeleteResult(
            IErrorLogProvider provider,
            Guid[] ids) : base(provider)
        {
            this.ids = ids;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if(ids != null)
                foreach(var id in ids)
                    Provider.Delete(id);
            base.ExecuteResult(context);
        }

        readonly Guid[] ids;
    }
}