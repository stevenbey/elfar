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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ErrorLog/Plugins.cshtml")]
    public class Plugins : System.Web.Mvc.WebViewPage<IErrorLogPlugin[]>
    {
        public Plugins()
        {
        }
        public override void Execute()
        {


            
            #line 2 "..\..\Views\ErrorLog\Plugins.cshtml"
 foreach(var plugin in Model)
{
    Html.RenderPartial(plugin.Name());
}
            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
