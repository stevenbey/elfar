// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Elfar.Mvc.Resources
{
    public class Provider : VirtualPathProvider
    {
        static Provider()
        {
            files = Components.Assemblies.SelectMany(a => a.GetManifestResourceNames().Select(n => new File(n, a))).ToDictionary(f => f.VirtualPath);
        }

        public override bool FileExists(string virtualPath)
        {
            return IsResource(virtualPath) || base.FileExists(virtualPath);
        }
        public override VirtualFile GetFile(string virtualPath)
        {
            return IsResource(virtualPath) ? files[virtualPath] : base.GetFile(virtualPath);
        }
        
        static bool IsResource(string virtualPath)
        {
            return files.ContainsKey(VirtualPathUtility.ToAbsolute(virtualPath));
        }

        static readonly IDictionary<string, File> files;

        class File : VirtualFile
        {
            public File(string resource, Assembly assembly) : base(ToVirtualPath(resource, assembly))
            {
                this.assembly = assembly;
                this.resource = resource;
            }
         
            public override Stream Open()
            {
                return assembly.GetManifestResourceStream(resource);
            }

            static string ToVirtualPath(string n, Assembly a)
            {
                return "/elfar" + Path.GetFileNameWithoutExtension(n).Replace(".", "/").Substring(a.GetName().Name.Length) + Path.GetExtension(n);
            }

            readonly Assembly assembly;
            readonly string resource;
        }
    }
}