using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Elfar.Mvc.ActionResults
{
    internal class EmbeddedResourceResult : FileStreamResult
    {
        public EmbeddedResourceResult(string filename, string ext)
            : base(GetStream(filename, ext), "text/" + ext) { }

        static Stream GetStream(string filename, string ext)
        {
            var assembly = typeof(EmbeddedResourceResult).Assembly;
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(filename + "." + ext, StringComparison.OrdinalIgnoreCase));
            var s = resourceName == null ? null : assembly.GetManifestResourceStream(resourceName);
            return s;
        }
    }
}