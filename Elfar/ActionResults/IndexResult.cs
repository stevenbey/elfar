using System;
using System.Web.Mvc;
using Elfar.Models;

namespace Elfar.ActionResults
{
    public class IndexResult
        : ViewResult
    {
        public IndexResult(
            int page,
            int size,
            IErrorLogProvider provider,
            Func<string, object, RedirectToRouteResult> redirect)
        {
            this.page = page;
            this.size = size;
            this.provider = provider;
            this.redirect = redirect;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if(page < 1 || size < 10)
            {
                if(page < 1) page = 1;
                if(size < 10) size = 10;
                Redirect(context);
            }
            Index model;
            try
            {
                model = new Index(page, size)
                {
                    Application = provider.Application,
                    Errors = provider.List(page - 1, size),
                    Total = provider.Total
                };
            }
            catch(Exception e)
            {
                throw new ErrorLogException(e);
            }
            ViewData.Model = model;
            if(page > model.Pages)
            {
                page = model.Pages;
                Redirect(context);
            }
            base.ExecuteResult(context);
        }

        void Redirect(ControllerContext context)
        {
            redirect(null, new { page, size }).ExecuteResult(context);
        }

        int page;
        readonly IErrorLogProvider provider;
        readonly Func<string, object, RedirectToRouteResult> redirect;
        int size;
    }
}