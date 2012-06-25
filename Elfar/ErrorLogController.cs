using System;
using System.Web.Mvc;
using Elfar.ActionResults;
using Elfar.Models;

namespace Elfar
{
    class ErrorLogController
        : AsyncController
    {
        public ErrorLogController(
            IErrorLogProvider provider)
        {
            this.provider = provider;
        }

        public DefaultResult Default(Guid id)
        {
            return new DefaultResult(id, provider, e => View(new Default { ErrorLog = e }));
        }
        public RssResult Digest()
        {
            return new RssResult(900, provider);
        }
        public void DownloadAsync()
        {
            AsyncManager.OutstandingOperations.Increment();
            var csv = new Csv(provider, Request.Url);
            csv.Complete += (src, e) =>
            {
                AsyncManager.Parameters["csv"] = e.UserState;
                AsyncManager.OutstandingOperations.Decrement();
            };
            csv.Load();
        }
        public CsvResult DownloadCompleted(Csv csv)
        {
            return new CsvResult { Data = csv };
        }
        public DefaultResult Html(Guid id)
        {
            return new DefaultResult(id, provider, e => Content(e.Html));
        }
        public IndexResult Index(int page = 1, int size = 15)
        {
            return new IndexResult(page, size, provider, RedirectToAction);
        }
        public DefaultResult Json(Guid id)
        {
            return new DefaultResult(id, provider, e => Json(e, JsonRequestBehavior.AllowGet));
        }
        public RssResult Rss()
        {
            return new RssResult(15, provider);
        }
        public ViewResult Stylesheet()
        {
            return View();
        }
        public void Test()
        {
            throw new TestException();
        }
        public DefaultResult Xml(Guid id)
        {
            return new DefaultResult(id, provider, e => new XmlResult { Data = e });
        }

        readonly IErrorLogProvider provider;
    }
}