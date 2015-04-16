using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Routing;

namespace Elfar.Web.Mvc
{
    using dictionary = System.Collections.Generic.Dictionary<string, object>;

    class ErrorLog : Elfar.ErrorLog
    {
        public ErrorLog(Exception exception, RouteData data, HttpContextBase context) : base(exception)
        {
            var httpException = exception as HttpException;

            if(httpException != null)
            {
                Code = httpException.GetHttpCode();
                Html = Regex.Replace(httpException.GetHtmlErrorMessage() ?? "", @"\s*[<>{}]\s*", m => m.Value.Trim());
            }

            if(data != null)
            {
                var route = data.Route as Route;
                if (route != null)
                {
                    RouteConstraints = new dictionary(route.Constraints); ;
                    RouteDefaults = new dictionary(route.Defaults);
                    RouteUrl = route.Url;
                }

                Action = data.GetRequiredString("action").ToPascal();
                Controller = data.GetRequiredString("controller").ToPascal();

                RouteData = new dictionary(data.Values);
                DataTokens = new dictionary(data.DataTokens);

                if(DataTokens.ContainsKey("area")) Area = DataTokens["area"].ToPascal();
            }

            if(context == null)
            {
                Cookies =
                Form =
                QueryString =
                ServerVariables = Dictionary.Empty;
                return;
            }

            try { Host = context.Server.MachineName; }
            catch(HttpException) { }

            var user = context.User;
            if(!(user == null || string.IsNullOrWhiteSpace(user.Identity.Name))) User = user.Identity.Name;

            var request = context.Request;

            Url = request.Url;
            HttpMethod = request.HttpMethod;

            var unvalidated = request.Unvalidated();

            Cookies = (Dictionary) request.Cookies;
            Form = (Dictionary) unvalidated.Form;
            QueryString = (Dictionary) unvalidated.QueryString;
            ServerVariables = (Dictionary) request.ServerVariables;
        }
    }
}