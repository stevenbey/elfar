﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
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
    using Elfar.Mvc.Properties;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Emails/ErrorLog.Text.cshtml")]
    public partial class ErrorLog_Text : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ErrorLog_Text()
        {
        }
        public override void Execute()
        {

WriteLiteral("Content-Type: text/plain\n\n");


            
            #line 4 "..\..\Views\Emails\ErrorLog.Text.cshtml"
Write(Model.Subject);

            
            #line default
            #line hidden
WriteLiteral("\n");


            
            #line 5 "..\..\Views\Emails\ErrorLog.Text.cshtml"
 if(Model.Time > DateTime.MinValue)
{

            
            #line default
            #line hidden
WriteLiteral("Generated: ");


            
            #line 7 "..\..\Views\Emails\ErrorLog.Text.cshtml"
        Write(Model.Time.ToString("r"));

            
            #line default
            #line hidden
WriteLiteral("\n");


            
            #line 8 "..\..\Views\Emails\ErrorLog.Text.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("Source:\n");


            
            #line 10 "..\..\Views\Emails\ErrorLog.Text.cshtml"
Write(Model.Source);

            
            #line default
            #line hidden
WriteLiteral("\n\n");


            
            #line 12 "..\..\Views\Emails\ErrorLog.Text.cshtml"
Write(AssemblyInfo.Value);

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
