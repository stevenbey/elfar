﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ErrorLog/Scripts.cshtml")]
    public class Scripts : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Scripts()
        {
        }
        public override void Execute()
        {
WriteLiteral("        <script type=\"text/javascript\" src=\"");


            
            #line 1 "..\..\Views\ErrorLog\Scripts.cshtml"
                                       Write(Url.Action("JavaScript"));

            
            #line default
            #line hidden
WriteLiteral("\"></script>\r\n        <script type=\"text/javascript\" src=\"");


            
            #line 2 "..\..\Views\ErrorLog\Scripts.cshtml"
                                       Write(Url.Action("JavaScript"));

            
            #line default
            #line hidden
WriteLiteral("?file=Linq\"></script>\r\n        <script type=\"text/javascript\" src=\"");


            
            #line 3 "..\..\Views\ErrorLog\Scripts.cshtml"
                                       Write(Url.Action("JavaScript"));

            
            #line default
            #line hidden
WriteLiteral("?file=Index\"></script>\r\n        <script type=\"text/javascript\" src=\"");


            
            #line 4 "..\..\Views\ErrorLog\Scripts.cshtml"
                                       Write(Url.Action("JavaScript"));

            
            #line default
            #line hidden
WriteLiteral("?file=HighChart\" defer=\"defer\"></script>");


        }
    }
}
#pragma warning restore 1591
