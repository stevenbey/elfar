using System;
using System.Web.Mvc;
using Elfar.Models;

namespace Elfar.ActionResults
{
    public class RssResult
        : ViewResult
    {
        public RssResult(
            int size,
            IErrorLogProvider provider)
        {
            this.size = size;
            this.provider = provider;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/xml";
            try
            {
                ViewData.Model = new Index(1, size)
                {
                    Application = provider.Application,
                    Errors = provider.List(0, size),
                    Total = provider.Total
                };
            }
            catch(Exception e)
            {
                throw new ErrorLogException(e);
            }
            base.ExecuteResult(context);
        }

        readonly int size;
        readonly IErrorLogProvider provider;
    }
}