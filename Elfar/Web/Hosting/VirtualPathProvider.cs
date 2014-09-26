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
            resources = Composition.Assemblies.SelectMany(a => a.GetManifestResourceNames().Select(n => new Resource(n, a.GetManifestResourceStream)));
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

        static bool IsMatch(Resource resource, string virtualPath)
        {
            if (resource.VirtualPath == null && ((virtualPath[0] == '/' && resource.Name.EndsWith(virtualPath.Replace("/", "."))) || resource.Name == virtualPath)) resource.VirtualPath = virtualPath;
            return resource.VirtualPath == virtualPath;
        }

        Resource this[string virtualPath]
        {
            get { return resources.FirstOrDefault(r => IsMatch(r, virtualPath)); }
        }

        static readonly IEnumerable<Resource> resources;

        class Resource
        {
            public Resource(string name, Func<string, Stream> func)
            {
                this.func = func;
                Name = name;
            }

            public string Name { get; private set; }
            public Stream Stream
            {
                get { return func(Name); }
            }
            public VirtualFile VirtualFile { get; private set; }
            public string VirtualPath
            {
                get { return virtualPath; }
                set
                {
                    virtualPath = value;
                    VirtualFile = new VirtualFile(value, this);
                }
            }

            readonly Func<string, Stream> func;
            string virtualPath;
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