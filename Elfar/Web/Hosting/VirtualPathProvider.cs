using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Caching;

namespace Elfar.Web.Hosting
{
    class VirtualPathProvider : System.Web.Hosting.VirtualPathProvider
    {
        public VirtualPathProvider(Assembly assembly)
        {
            getManifestResourceStream = assembly.GetManifestResourceStream;
            manifestResourceNames = assembly.GetManifestResourceNames();
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
            return this[virtualPath] ?? base.GetFile(virtualPath);
        }

        VirtualFile this[string virtualPath]
        {
            get
            {
                lock (key) if (virtualFiles.ContainsKey(virtualPath)) return virtualFiles[virtualPath];
                var manifestResourceName = virtualPath.Replace("/", ".");
                manifestResourceName = manifestResourceNames.FirstOrDefault(n => n.EndsWith(manifestResourceName));
                if (manifestResourceName == null) return null;
                var virtualFile = new VirtualFile(virtualPath, manifestResourceName, getManifestResourceStream);
                lock (key) virtualFiles.Add(virtualPath, virtualFile);
                return virtualFile;
            }
        }

        readonly Func<string, Stream> getManifestResourceStream;
        static readonly object key = new object();
        readonly string[] manifestResourceNames;
        readonly Dictionary<string, VirtualFile> virtualFiles = new Dictionary<string, VirtualFile>();

        class VirtualFile : System.Web.Hosting.VirtualFile
        {
            public VirtualFile(string virtualPath, string manifestResourceName, Func<string, Stream> getManifestResourceStream) : base(virtualPath)
            {
                this.manifestResourceName = manifestResourceName;
                this.getManifestResourceStream = getManifestResourceStream;
            }

            public override Stream Open()
            {
                return getManifestResourceStream(manifestResourceName);
            }
            public override string ToString()
            {
                using (var reader = new StreamReader(Open()))
                    return reader.ReadToEnd();
            }

            readonly string manifestResourceName;
            readonly Func<string, Stream> getManifestResourceStream;
        }
    }
}