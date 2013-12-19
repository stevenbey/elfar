﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Elfar.Mvc.Views.ErrorLog
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Elfar;
    using Elfar.Mvc;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ErrorLog/Default.cshtml")]
    public class Default : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Default()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n    <head>\r\n        <title>Error Logging Filter and Rout" +
"e (ELFAR)</title>\r\n        ");



WriteLiteral(@"
        <style type=""text/css"">
            body
            {
                width: 100%;
                margin: 0;
                padding: 0;
                font-family: verdana;
                overflow-x: hidden;
                overflow-y: auto;
            }
            header, footer
            {
                position: fixed;
                width: 100%;
                padding: 10px;
                background-color: #0072C6;
                color: white;
                z-index: 999;
            }
            header
            {
                height: 50px;
            }
            footer
            {
                bottom: 0;
                height: 16px;
                font-size: small;
                text-align: center;
            }

            pre
            {
                width: 95%;
            }

            #content
            {
                position: absolute;
                top: 60px;
                width: 100%;
                padding: 10px;
                padding-bottom: 36px;
                overflow-x: auto;
                overflow-y: hidden;
            }
        </style>
    </head>
    <body>
        <header>Error Logs for ");


            
            #line 55 "..\..\Views\ErrorLog\Default.cshtml"
                           Write(ErrorLogProvider.Settings.Application ?? "''");

            
            #line default
            #line hidden
WriteLiteral(" on ");


            
            #line 55 "..\..\Views\ErrorLog\Default.cshtml"
                                                                              Write(Server.MachineName);

            
            #line default
            #line hidden
WriteLiteral("</header>\r\n        <div id=\"content\">\r\n            <pre id=\"stack-trace\">\r\n      " +
"        \r\n            </pre>\r\n        </div>\r\n        <footer>Beyond395 Limited " +
"&copy; 2011&ndash;");


            
            #line 61 "..\..\Views\ErrorLog\Default.cshtml"
                                               Write(DateTime.Now.Year);

            
            #line default
            #line hidden

            
            #line 61 "..\..\Views\ErrorLog\Default.cshtml"
                                                                      WriteLiteral(" &nbsp; ");

            
            #line default
            #line hidden
            
            #line 61 "..\..\Views\ErrorLog\Default.cshtml"
                                                                               if(!string.IsNullOrWhiteSpace(ErrorLogProvider.Name)) {
            
            
            #line default
            #line hidden
            
            #line 62 "..\..\Views\ErrorLog\Default.cshtml"
       Write(ErrorLogProvider.Name);

            
            #line default
            #line hidden

WriteLiteral(" ");

WriteLiteral("(v");


            
            #line 62 "..\..\Views\ErrorLog\Default.cshtml"
                                   Write(ErrorLogProvider.Version);

            
            #line default
            #line hidden
WriteLiteral(")\r\n");


            
            #line 63 "..\..\Views\ErrorLog\Default.cshtml"
        }
            
            #line default
            #line hidden
WriteLiteral("</footer>\r\n        <script src=\"");


            
            #line 64 "..\..\Views\ErrorLog\Default.cshtml"
                Write(Url.Content("~/elfar/Resources/Scripts/JavaScript.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"></script>\r\n        <script type=\"text/javascript\">\r\n    " +
"        $(function() {\r\n                $.ajax({ url: \"");


            
            #line 67 "..\..\Views\ErrorLog\Default.cshtml"
                          Write(Url.Content("~/elfar/Errors"));

            
            #line default
            #line hidden
WriteLiteral("\" }).done(function(json) {\r\n                    $(\"pre#stack-trace\").html(json[0]" +
".StackTrace);\r\n                });\r\n            });\r\n        </script>\r\n    </bo" +
"dy>\r\n</html>");


        }
    }
}
#pragma warning restore 1591
