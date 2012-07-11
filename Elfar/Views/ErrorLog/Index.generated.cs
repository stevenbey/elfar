﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Elfar.Views.ErrorLog
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
    
    #line 1 "..\..\Views\ErrorLog\Index.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\ErrorLog\Index.cshtml"
    using Elfar;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.2.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ErrorLog/Index.cshtml")]
    public class Index : System.Web.Mvc.WebViewPage<Elfar.Models.Index>
    {
        public Index()
        {
        }
        public override void Execute()
        {



WriteLiteral(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
    <head>
        <meta http-equiv=""X-UA-Compatible"" content=""IE=EmulateIE7"" />
        <title>Error log for [");


            
            #line 8 "..\..\Views\ErrorLog\Index.cshtml"
                         Write(Model.Application);

            
            #line default
            #line hidden
WriteLiteral("] on ");


            
            #line 8 "..\..\Views\ErrorLog\Index.cshtml"
                                                Write(Server.MachineName);

            
            #line default
            #line hidden
WriteLiteral("</title>\r\n        <link rel=\"stylesheet\" type=\"text/css\" href=\"");


            
            #line 9 "..\..\Views\ErrorLog\Index.cshtml"
                                                Write(Url.Action("Stylesheet"));

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n        <link rel=\"alternate\" type=\"application/rss+xml\" title=\"RSS\" href=\"" +
"");


            
            #line 10 "..\..\Views\ErrorLog\Index.cshtml"
                                                                      Write(Url.Action("Rss"));

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n    </head>\r\n    <body>\r\n        <h1>Error Log for <span>[");


            
            #line 13 "..\..\Views\ErrorLog\Index.cshtml"
                            Write(Model.Application);

            
            #line default
            #line hidden
WriteLiteral("] on ");


            
            #line 13 "..\..\Views\ErrorLog\Index.cshtml"
                                                   Write(Server.MachineName);

            
            #line default
            #line hidden
WriteLiteral("</span></h1>\r\n");


            
            #line 14 "..\..\Views\ErrorLog\Index.cshtml"
     if(Model.Errors.Count != 0)
    {

            
            #line default
            #line hidden
WriteLiteral("        <ul id=\"navigation\">\r\n            <li><a href=\"");


            
            #line 17 "..\..\Views\ErrorLog\Index.cshtml"
                    Write(Url.Action("Rss"));

            
            #line default
            #line hidden
WriteLiteral("\" title=\"RSS feed of recent errors\">RSS Feed</a></li>\r\n            <li><a href=\"");


            
            #line 18 "..\..\Views\ErrorLog\Index.cshtml"
                    Write(Url.Action("Digest"));

            
            #line default
            #line hidden
WriteLiteral("\" title=\"RSS feed of errors within recent days\">RSS Digest</a></li>\r\n            " +
"<li><a href=\"");


            
            #line 19 "..\..\Views\ErrorLog\Index.cshtml"
                    Write(Url.Action("Download"));

            
            #line default
            #line hidden
WriteLiteral("\" title=\"Download the entire log as CSV\">Download</a></li>\r\n        </ul>\r\n");


            
            #line 21 "..\..\Views\ErrorLog\Index.cshtml"
        using(Html.BeginForm())
        {

            
            #line default
            #line hidden
WriteLiteral(@"            <table>
                <thead>
                    <tr>
                        <th><input type=""checkbox"" id=""select-all"" /></th>
                        <th title=""Number"">#</th>
                        <th>Host</th>
                        <th>Application</th>
                        <th>Code</th>
                        <th>Type</th>
                        <th>Error</th>
                        <th>Date &amp; Time</th>
                    </tr>
                </thead>
                <tbody>
");


            
            #line 37 "..\..\Views\ErrorLog\Index.cshtml"
                   var i = 1; 

            
            #line default
            #line hidden

            
            #line 38 "..\..\Views\ErrorLog\Index.cshtml"
                 foreach(var errorLog in Model.Errors)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <tr>\r\n                        <td><input type=\"checkbox\" valu" +
"e=\"");


            
            #line 41 "..\..\Views\ErrorLog\Index.cshtml"
                                                     Write(errorLog.ID);

            
            #line default
            #line hidden
WriteLiteral("\" /></td>\r\n                        <td>");


            
            #line 42 "..\..\Views\ErrorLog\Index.cshtml"
                        Write(i++);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                        <td>");


            
            #line 43 "..\..\Views\ErrorLog\Index.cshtml"
                       Write(errorLog.Host);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                        <td>");


            
            #line 44 "..\..\Views\ErrorLog\Index.cshtml"
                       Write(errorLog.Application);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                        <td title=\"");


            
            #line 45 "..\..\Views\ErrorLog\Index.cshtml"
                              Write(HttpWorkerRequest.GetStatusDescription(errorLog.Code.GetValueOrDefault()));

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 45 "..\..\Views\ErrorLog\Index.cshtml"
                                                                                                          Write(errorLog.Code);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                        <td title=\"");


            
            #line 46 "..\..\Views\ErrorLog\Index.cshtml"
                              Write(errorLog.Type);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 46 "..\..\Views\ErrorLog\Index.cshtml"
                                              Write(errorLog.Type.ShortName());

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                        <td>");


            
            #line 47 "..\..\Views\ErrorLog\Index.cshtml"
                       Write(errorLog.Message);

            
            #line default
            #line hidden
WriteLiteral(" ");


            
            #line 47 "..\..\Views\ErrorLog\Index.cshtml"
                                         Write(Html.ActionLink("Details…", "Default", "ErrorLog", new { id = errorLog.ID }, null));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                        <td nowrap=\"nowrap\" title=\"");


            
            #line 48 "..\..\Views\ErrorLog\Index.cshtml"
                                              Write(errorLog.Time);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 48 "..\..\Views\ErrorLog\Index.cshtml"
                                                              Write(errorLog.Time);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    </tr>\r\n");


            
            #line 50 "..\..\Views\ErrorLog\Index.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                </tbody>\r\n            </table>\r\n");



WriteLiteral("            <p><input type=\"submit\" id=\"delete\" name=\"Action::Delete\" value=\"Dele" +
"te\" class=\"ui-button ui-state-default ui-corner-all\" disabled=\"disabled\" /></p>\r" +
"\n");


            
            #line 54 "..\..\Views\ErrorLog\Index.cshtml"
        }
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        <p>No errors found.</p>\r\n");


            
            #line 59 "..\..\Views\ErrorLog\Index.cshtml"
    }

            
            #line default
            #line hidden

            
            #line 60 "..\..\Views\ErrorLog\Index.cshtml"
   Html.RenderPartial("Footer"); 

            
            #line default
            #line hidden
WriteLiteral("        <script type=\"text/javascript\" src=\"");


            
            #line 61 "..\..\Views\ErrorLog\Index.cshtml"
                                       Write(Url.Action("JavaScript"));

            
            #line default
            #line hidden
WriteLiteral("\"></script>\r\n        <script type=\"text/javascript\" src=\"");


            
            #line 62 "..\..\Views\ErrorLog\Index.cshtml"
                                       Write(Url.Action("JavaScript"));

            
            #line default
            #line hidden
WriteLiteral("?file=Index\"></script>\r\n    </body>\r\n</html>");


        }
    }
}
#pragma warning restore 1591
