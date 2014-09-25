using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Caching;

namespace Elfar.Web.Hosting
{
    class VirtualPathProvider : System.Web.Hosting.VirtualPathProvider
    {
        static VirtualPathProvider()
        {
            resources = Components.Assemblies.SelectMany(a => a.GetManifestResourceNames().Select(n => new Resource(n, a.GetManifestResourceStream)));
        }
        
        public override bool FileExists(string virtualPath)
        {
            return this[virtualPath] != null || base.FileExists(virtualPath);
        }
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return null;
        }
        public override System.Web.Hosting.VirtualFile GetFile(string virtualPath)
        {
            var resource = this[virtualPath];
            return resource == null ? base.GetFile(virtualPath) : resource.VirtualFile;
        }

        static bool IsMatch(string resource, string virtualPath)
        {
            return virtualPath.StartsWith("/") 
                ? resource.EndsWith(virtualPath.Replace("/", "."), StringComparison.OrdinalIgnoreCase)
                : resource == virtualPath;
        }

        Resource this[string virtualPath]
        {
            get
            {
                var resource = resources.FirstOrDefault(r => IsMatch(r.Name, virtualPath));
                if (resource != null && resource.VirtualFile == null) resource.SetVirtualFile(virtualPath);
                return resource;
            }
        }

        static readonly IEnumerable<Resource> resources;

        class Resource
        {
            public Resource(string name, Func<string, Stream> func)
            {
                this.func = func;
                Name = name;
            }

            public void SetVirtualFile(string virtualPath)
            {
                VirtualFile = new VirtualFile(virtualPath, this);
            }

            public string Name { get; private set; }
            public Stream Stream
            {
                get { return func(Name); }
            }
            public VirtualFile VirtualFile { get; private set; }
            
            readonly Func<string, Stream> func;
        }

        class VirtualFile : System.Web.Hosting.VirtualFile
        {
            public VirtualFile(string virtualPath, Resource resource) : base(virtualPath)
            {
                this.resource = resource;
            }

            public override Stream Open()
            {
                return resource.Stream;
            }
            
            readonly Resource resource;
        }
    }
}