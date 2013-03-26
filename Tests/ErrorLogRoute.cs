using System;
using System.Web.Routing;
using Beyond.Tests.Mvc;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Elfar.Tests
{
    [TestFixture]
    public class ErrorLogRoute
    {
        [Test]
        public void Index_GetRouteData()
        {
            Run_GetRouteData("/elfar", "Index");
        }

        [Test]
        public void Index_VirtualPathData()
        {
            Run_VirtualPathData("Index", "elfar");
        }

        [Test]
        public void Default_GetRouteData()
        {
            Run_GetRouteData("Default");
        }

        [Test]
        public void Default_VirtualPathData()
        {
            Run_VirtualPathData("Default");
        }

        [Test]
        public void Stylesheet_GetRouteData()
        {
            Run_GetRouteData("/elfar/stylesheet", "stylesheet");
        }

        [Test]
        public void Stylesheet_VirtualPathData()
        {
            Run_VirtualPathData("Stylesheet", "elfar/Stylesheet");
        }

        [Test]
        public void Rss_GetRouteData()
        {
            Run_GetRouteData("/elfar/Rss", "Rss");
        }

        [Test]
        public void Rss_VirtualPathData()
        {
            Run_VirtualPathData("Rss", "elfar/Rss");
        }

        [Test]
        public void Digest_GetRouteData()
        {
            Run_GetRouteData("/elfar/Digest", "Digest");
        }

        [Test]
        public void Digest_VirtualPathData()
        {
            Run_VirtualPathData("Digest", "elfar/Digest");
        }

        [Test]
        public void Download_GetRouteData()
        {
            Run_GetRouteData("/elfar/Download", "Download");
        }

        [Test]
        public void Download_VirtualPathData()
        {
            Run_VirtualPathData("Download", "elfar/Download");
        }

        [Test]
        public void Test_VirtualPathData()
        {
            Run_VirtualPathData("Test", "elfar/Test");
        }

        [Test]
        public void Xml_GetRouteData()
        {
            Run_GetRouteData("Xml");
        }

        [Test]
        public void Xml_VirtualPathData()
        {
            Run_VirtualPathData("Xml");
        }

        [Test]
        public void Json_GetRouteData()
        {
            Run_GetRouteData("Json");
        }

        [Test]
        public void Json_VirtualPathData()
        {
            Run_VirtualPathData("Json");
        }

        [Test]
        public void Setup_GetRouteData()
        {
            Run_GetRouteData("/elfar/Setup", "Setup");
        }

        [Test]
        public void Setup_VirtualPathData()
        {
            Run_VirtualPathData("Setup", "elfar/Setup");
        }

        [Test]
        public void NotElfar_GetRouteData()
        {
            // Arrange
            var route = new Elfar.ErrorLogRoute(null);
            var httpContext = new HttpContext("/");

            // Act
            var data = route.GetRouteData(httpContext);

            // Assert
            Assert.That(data, Is.Null);
        }

        [Test]
        public void NotElfar_VirtualPathData()
        {
            // Arrange
            var route = new Elfar.ErrorLogRoute(null);
            var request = Request;

            request.RouteData.Values["controller"] = "Default";
            request.RouteData.Values["action"] = "Default";

            // Act
            var vpd = route.GetVirtualPath(request, request.RouteData.Values);

            // Assert
            Assert.That(vpd, Is.Null);
        }

        static void Run_GetRouteData(string action)
        {
            // Arrange
            var route = new Elfar.ErrorLogRoute(null);
            var httpContext = new HttpContext("/elfar/" + Guid.Empty + (action == "Default" ? null : "/" + action));

            // Act
            var data = route.GetRouteData(httpContext);

            // Assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Values["id"], Is.EqualTo(Guid.Empty.ToString()));
            Assert.That(data.Values["action"], Is.EqualTo(action));
        }
        static void Run_GetRouteData(string url, string action)
        {
            // Arrange
            var route = new Elfar.ErrorLogRoute(null);
            var httpContext = new HttpContext(url);

            // Act
            var data = route.GetRouteData(httpContext);

            // Assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Values["action"], Is.EqualTo(action));
        }
        static void Run_VirtualPathData(string action)
        {
            // Arrange
            var route = new Elfar.ErrorLogRoute(null);
            var request = Request;

            request.RouteData.Values["id"] = Guid.Empty;
            request.RouteData.Values["action"] = action;

            // Act
            var vpd = route.GetVirtualPath(request, request.RouteData.Values);

            // Assert
            Assert.That(vpd, Is.Not.Null);
            Assert.That(vpd.VirtualPath, Is.EqualTo("elfar/00000000-0000-0000-0000-000000000000" + (action == "Default" ? null : "/" + action)));
        }
        static void Run_VirtualPathData(string action, string url)
        {
            // Arrange
            var route = new Elfar.ErrorLogRoute(null);
            var request = Request;

            request.RouteData.Values.Add("action", action);

            // Act
            var vpd = route.GetVirtualPath(request, request.RouteData.Values);

            // Assert
            Assert.That(vpd, Is.Not.Null);
            Assert.That(vpd.VirtualPath, Is.EqualTo(url));
        }

        static RequestContext Request
        {
            get
            {
                var request = new RequestContext { RouteData = new RouteData() };
                request.RouteData.Values.Add("controller", "ErrorLog");
                return request;
            }
        }
    }
}
// ReSharper restore InconsistentNaming