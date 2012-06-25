namespace Elfar.Views
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.WebPages;

    public class View
        : IView
    {
        public View(
            string virtualPath,
            Type type)
        {
            this.type = type;
            this.virtualPath = virtualPath;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            var webViewPage = Activator.CreateInstance(type) as WebViewPage;
            if(webViewPage == null) throw new InvalidOperationException("Invalid view type");
            webViewPage.VirtualPath = virtualPath;
            webViewPage.ViewContext = viewContext;
            webViewPage.ViewData = viewContext.ViewData;
            webViewPage.InitHelpers();
            webViewPage.ExecutePageHierarchy(new WebPageContext(viewContext.HttpContext, webViewPage, null), writer, null);
        }

        readonly Type type;
        readonly string virtualPath;
    }
}