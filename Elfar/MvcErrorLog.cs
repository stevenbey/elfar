using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Elfar
{
    public class MvcErrorLog
        : ErrorLog
    {
        public MvcErrorLog() {}

        public MvcErrorLog(
            string application,
            ExceptionContext exceptionContext)
            : base(application, exceptionContext.Exception)
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
            
            RemoveDataTablesCookie();
            RemoveDataTablesCookie(allHttp);
            RemoveDataTablesCookie(allRaw);
            RemoveDataTablesCookie(httpCookie);
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

        void RemoveDataTablesCookie(string key = null)
        {
            if(key == null)
            {
                Cookies.Remove("SpryMedia_DataTables_DataTables_Table_0_elfar");
                return;
            }
            if(!ServerVariables.ContainsKey(key)) return;
            ServerVariables[key] = Regex.Replace(ServerVariables[key], @"SpryMedia_DataTables_DataTables_Table_0_elfar=[\w\W]*?((;\s)|(?=\n)|$)", "");
            ServerVariables[key] = Regex.Replace(ServerVariables[key], @"(http_)?cookie:\s?\n", "", RegexOptions.IgnoreCase);
        }

        static string TryGetMachineName(HttpContextBase context)
        {
            try { return context.Server.MachineName; }
            catch (HttpException) { }
            catch (SecurityException) { }
            return null;
        }

        const string allHttp = "ALL_HTTP";
        const string allRaw = "ALL_RAW";
        const string httpCookie = "HTTP_COOKIE";
    }
}