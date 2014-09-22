using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc
{
    class ErrorLog : Elfar.ErrorLog
    {
        public ErrorLog(Exception exception, RouteData data, HttpContextBase context) : base(exception)
        {
            var httpException = exception as HttpException;

            if(httpException != null)
            {
                Code = httpException.GetHttpCode();
                Html = httpException.GetHtmlErrorMessage();
                if (Html != null)
                {
                    Html = Regex.Replace(Regex.Replace(Html, @">\s+", ">"), @"\s+<", "<"); // remove spaces/new lines between tags
                    Html = Regex.Replace(Regex.Replace(Html, @"}\s+", "}"), @"\s+{", "{"); // remove spaces/new lines between style rules
                    Html = Regex.Replace(Regex.Replace(Html, @"\s+}", "}"), @"{\s+", "{"); // remove spaces/new lines between style media rules
                }
            }

            if(data != null)
            {
                var route = data.Route as Route;
                if (route != null)
                {
                    RouteConstraints = route.Constraints;
                    RouteDefaults = route.Defaults;
                    RouteUrl = route.Url;
                }

                Action = data.GetRequiredString("action").ToPascal();
                Controller = data.GetRequiredString("controller").ToPascal();

                RouteData = data.Values;
                DataTokens = data.DataTokens;

                if(DataTokens.ContainsKey("area")) Area = DataTokens["area"].ToPascal();
            }

            if(context == null)
            {
                Cookies = Form = QueryString = ServerVariables = Dictionary.Empty;
                return;
            }

            try { Host = context.Server.MachineName; }
            catch(HttpException) { }

            var user = context.User;
            if(!(user == null || string.IsNullOrWhiteSpace(user.Identity.Name))) User = user.Identity.Name;

            var request = context.Request;

            Url = request.Url;
            HttpMethod = request.HttpMethod;

            Cookies = (Dictionary) request.Cookies;
            Form = (Dictionary) request.Form;
            QueryString = (Dictionary) request.QueryString;
            ServerVariables = (Dictionary) request.ServerVariables;
        }
    }
}