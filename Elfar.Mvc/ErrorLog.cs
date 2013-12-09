using System.Security;
using System.Web;
using System.Web.Mvc;

namespace Elfar.Mvc
{
    class ErrorLog : Elfar.ErrorLog
    {
        public ErrorLog()
        {
            Cookies = new Collection();
            Form = new Collection();
            QueryString = new Collection();
            ServerVariables = new Collection();
        }
        public ErrorLog(string application, ExceptionContext exceptionContext) : base(application, exceptionContext.Exception)
        {
            var context = exceptionContext.HttpContext;

            if (context == null) return;

            Host = Host ?? GetMachineName(context);

            var user = context.User;
            if (!(user == null || string.IsNullOrWhiteSpace(user.Identity.Name)))
                User = user.Identity.Name;

            var request = context.Request;
            ServerVariables.Add(request.ServerVariables);
            QueryString.Add(request.QueryString);
            Form.Add(request.Form);
            Cookies.Add(request.Cookies);
        }

        static string GetMachineName(HttpContextBase context)
        {
            try { return context.Server.MachineName; }
            catch (HttpException) { }
            catch (SecurityException) { }
            return null;
        }

        public Collection Cookies { get; set; }
        public Collection Form { get; set; }
        public Collection QueryString { get; set; }
        public Collection ServerVariables { get; set; }
    }
}