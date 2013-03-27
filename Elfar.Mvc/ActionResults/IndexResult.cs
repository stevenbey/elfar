using System;
using System.Web.Mvc;
using Elfar.Mvc.Models;

namespace Elfar.Mvc.ActionResults
{
    public class IndexResult : ViewResult
    {
        public IndexResult(IErrorLogProvider provider, IErrorLogPlugin[] plugins)
        {
            this.plugins = plugins;
            Provider = provider;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            try
            {
                ViewData.Model = new Index
                {
                    Application = Provider.Application,
                    Errors = Provider.List(),
                    Plugins = plugins
                };
            }
            catch(Exception e)
            {
                throw new ErrorLogException(e);
            }
            base.ExecuteResult(context);
        }

        protected IErrorLogProvider Provider { get; private set; }
        
        readonly IErrorLogPlugin[] plugins;
    }
}