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
            Provider = provider;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            try
            {
                ViewData.Model = new Index
                {
                        Application = Provider.Application,
                        Errors = Provider.List()
                };
            }
            catch(Exception e)
            {
                throw new ErrorLogException(e);
            }
            base.ExecuteResult(context);
        }

        protected readonly IErrorLogProvider Provider;
    }
}