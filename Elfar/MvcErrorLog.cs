using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Elfar
{
    class MvcErrorLog : ErrorLog
    {
        public MvcErrorLog() {}

        public MvcErrorLog(string application, ExceptionContext exceptionContext) : base(application, exceptionContext.Exception)
        {
            var context = exceptionContext.HttpContext;

            if (context == null) return;

            Host = Host ?? TryGetMachineName(context);

            var user = context.User;
            if (!(user == null || string.IsNullOrWhiteSpace(user.Identity.Name)))
                User = user.Identity.Name;

            var request = context.Request;
            ServerVariables.Add(request.ServerVariables);
            QueryString.Add(request.QueryString);
            Form.Add(request.Form);
            Cookies.Add(request.Cookies);
        }

        public ErrorLog ToErrorLog()
        {
            return new ErrorLog
            {
                Application = Application,
                Code = Code,
                Cookies = Cookies,
                Detail = Detail,
                Form = Form,
                Host = Host,
                Html = Html,
                ID = ID,
                Message = Message,
                QueryString = QueryString,
                ServerVariables = ServerVariables,
                Source = Source,
                Time = Time,
                Type = Type,
                User = User
            };
        }

        static string TryGetMachineName(HttpContextBase context)
        {
            try { return context.Server.MachineName; }
            catch (HttpException) { }
            catch (SecurityException) { }
            return null;
        }
    }
}