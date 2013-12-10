using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Elfar.Mvc.Views
{
    public class Engine : VirtualPathProviderViewEngine, IVirtualPathFactory
    {
        public Engine()
        {
            AreaViewLocationFormats = new string[0];
            AreaMasterLocationFormats = new string[0];
            AreaPartialViewLocationFormats = new string[0];
            ViewLocationFormats = new[] { "~/Views/{1}/{0}.cshtml" };
            MasterLocationFormats = new[] { "~/Views/{1}/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Views/{1}/{0}.cshtml" };
            FileExtensions = new[] { "cshtml" };
            mappings = Components.Assemblies.SelectMany(a => a.GetTypes())
                                 .Where(typeof(WebViewPage).IsAssignableFrom)
                                 .Select(type => new { type, type.GetCustomAttributes(false).OfType<PageVirtualPathAttribute>().First().VirtualPath })
                                 .ToDictionary(p => p.VirtualPath, p => p.type, StringComparer.OrdinalIgnoreCase);
        }

        public object CreateInstance(string virtualPath)
        {
            Type type;
            return mappings.TryGetValue(virtualPath, out type) ? Activator.CreateInstance(type) : null;
        }
        public bool Exists(string virtualPath)
        {
            return mappings.ContainsKey(virtualPath);
        }
        
        protected override IView CreatePartialView(ControllerContext controllerContext, string path)
        {
            Type type;
            return mappings.TryGetValue(path, out type) ? new View(path, type) : null;
        }        
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return CreatePartialView(controllerContext, viewPath);
        }
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return Exists(virtualPath);
        }
        
        readonly IDictionary<string, Type> mappings;
    }
}