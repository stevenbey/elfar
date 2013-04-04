// ReSharper disable Mvc.ActionNotResolved
// ReSharper disable UnusedMember.Global
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Elfar.Mvc.ActionResults;
using Elfar.Mvc.Models;

namespace Elfar.Mvc
{
    class ErrorLogController : AsyncController
    {
        public ErrorLogController(IErrorLogProvider provider, IErrorLogPlugin[] plugins)
        {
            this.provider = provider;
            this.plugins = plugins;

            assemblies = plugins.Select(p => p.GetType().Assembly).ToArray();
        }
        
        public DefaultResult Default(Guid id)
        {
            return new DefaultResult(id, provider, e => View(new Default { ErrorLog = e }));
        }
        [HttpPost, FormAction]
        public RedirectToRouteResult Delete(IEnumerable<Guid> ids)
        {
            if(ids != null)
                foreach(var id in ids)
                    provider.Delete(id);
            return RedirectToAction("Index");
        }
        public RssResult Digest()
        {
            return new RssResult(provider);
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
        public FileStreamResult Image(string name)
        {
            return ResourceFile(name, "png", "image/png");
        }
        public IndexResult Index()
        {
            return new IndexResult(provider, plugins);
        }
        public new FileStreamResult JavaScript(string file = "JavaScript")
        {
            return ResourceFile(file, "js", "text/javascript");
        }
        public DefaultResult Json(Guid id, bool download = false)
        {
            if(download) Response.AddHeader("Content-Disposition", "attachment; filename=" + id + ".json;");
            return new DefaultResult(id, provider, e => Json(e, JsonRequestBehavior.AllowGet));
        }
        public RssResult Rss()
        {
            return new RssResult(provider);
        }
        public FileStreamResult Stylesheet(string file = "Stylesheet")
        {
            return ResourceFile(file, "css", "text/css");
        }
        public void Test()
        {
            throw new TestException();
        }
        public DefaultResult Xml(Guid id, bool download = false)
        {
            if(download) Response.AddHeader("Content-Disposition", "attachment; filename=" + id + ".xml;");
            return new DefaultResult(id, provider, e => new XmlResult { Data = e });
        }

        FileStreamResult ResourceFile(string name, string ext, string contentType)
        {
            var resource = ".Resources." + name + "." + ext;
            var stream = GetType().Assembly.GetManifestResourceStream("Elfar.Mvc" + resource) ??
                            assemblies.Select(a => a.GetManifestResourceStream(a.GetName().Name + resource)).FirstOrDefault();
            return stream == null ? null : File(stream, contentType);
        }

        readonly IErrorLogProvider provider;
        readonly IErrorLogPlugin[] plugins;
        readonly Assembly[] assemblies;
    }
}