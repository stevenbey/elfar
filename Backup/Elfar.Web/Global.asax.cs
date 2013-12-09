using System;

namespace Elfar.Web
{
    using System.Web;

    public class Global : HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            var application = (HttpApplication) sender;
            var error = application.Server.GetLastError();
            var httpException = error as HttpException;
            if(httpException != null)
            {
                var Code = httpException.GetHttpCode();
                var Html = httpException.GetHtmlErrorMessage();

                var code = Code;
                var html = Html;
            }
        }
    }
}