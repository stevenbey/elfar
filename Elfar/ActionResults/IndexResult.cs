using System;
using System.Web.Mvc;
using Elfar.Models;

namespace Elfar.ActionResults
{
    public class IndexResult
        : ViewResult
    {
        public IndexResult(IErrorLogProvider provider)
        {
            this.provider = provider;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            try
            {
                ViewData.Model = new Index
                {
                    Application = provider.Application,
                    Errors = provider.List()
                };
            }
            catch(Exception e)
            {
                throw new ErrorLogException(e);
            }
            base.ExecuteResult(context);
        }

        readonly IErrorLogProvider provider;
    }
}