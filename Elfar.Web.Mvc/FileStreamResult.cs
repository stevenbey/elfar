using System.IO;
using System.Web.Hosting;

namespace Elfar.Web.Mvc
{
    class FileStreamResult : System.Web.Mvc.FileStreamResult
    {
        public FileStreamResult(string virtualPath) : base(GetStream(virtualPath), GetContentType(virtualPath)) { }

        static string GetContentType(string virtualPath)
        {
            var ext = Path.GetExtension(virtualPath);
            return ext == null ? null : contentTypes[ext.TrimStart('.')];
        }
        static Stream GetStream(string virtualPath)
        {
            var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(virtualPath);
            return virtualFile == null ? null : virtualFile.Open();
        }

        static readonly Dictionary contentTypes = new Dictionary
        {
            { "css", "text/css" },
            { "js", "text/javascript" }
        };
    }
}