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

namespace Elfar.Mvc.Views.Emails
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Emails/ErrorLog.Html.cshtml")]
    public partial class ErrorLog_Html : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ErrorLog_Html()
        {
        }
        public override void Execute()
        {

WriteLiteral("Content-Type: text/html\r\n\r\n<html>\r\n\t<head>\r\n\t\t<title>");


            
            #line 6 "..\..\Views\Emails\ErrorLog.Html.cshtml"
    Write(Model.Subject);

            
            #line default
            #line hidden
WriteLiteral(@"</title>
		<style type=""text/css"">
            body { font-family: verdana, arial, helvetic; font-size: x-small; } 
            td, th, pre { font-size: x-small; } 
            #errorDetail { padding: 1em; background-color: #FFFFCC; } 
            #errorMessage { font-size: medium; font-style: italic; color: maroon; }
            h1 { font-size: small; }
        </style>
	</head>
	<body>
		<p id=""errorMessage"">");


            
            #line 16 "..\..\Views\Emails\ErrorLog.Html.cshtml"
                  Write(Model.Subject);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");


            
            #line 17 "..\..\Views\Emails\ErrorLog.Html.cshtml"
     if (Model.Time > DateTime.MinValue)
    {

            
            #line default
            #line hidden
WriteLiteral("        <p>Generated: ");


            
            #line 19 "..\..\Views\Emails\ErrorLog.Html.cshtml"
                 Write(Model.Time.ToString("r"));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");


            
            #line 20 "..\..\Views\Emails\ErrorLog.Html.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("        <p>");


            
            #line 21 "..\..\Views\Emails\ErrorLog.Html.cshtml"
      Write(Model.Source);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n        <pre id=\"errorDetail\">\r\n");


            
            #line 23 "..\..\Views\Emails\ErrorLog.Html.cshtml"
Write(Html.Raw(Model.StackTrace));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </pre>\r\n\t\t<p id=\"Footer\">Copyright &#169; 2012&ndash;");


            
            #line 25 "..\..\Views\Emails\ErrorLog.Html.cshtml"
                                        Write(DateTime.Now.Year);

            
            #line default
            #line hidden
WriteLiteral(" Beyond395 Limited &nbsp;&middot;&nbsp; All rights reserved &nbsp;&middot;&nbsp; " +
"");


            
            #line 25 "..\..\Views\Emails\ErrorLog.Html.cshtml"
                                                                                                                                           Write(ErrorLogProvider.Details);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n\t</body>\r\n</html>");


        }
    }
}
#pragma warning restore 1591