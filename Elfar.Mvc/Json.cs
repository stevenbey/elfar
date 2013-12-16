using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc
{
    class Json : Elfar.Json
    {
        public Json(Exception exception, RouteData data, HttpContextBase context) : base(exception)
        {
            var httpException = exception as HttpException;

            if(httpException != null)
            {
                Code = httpException.GetHttpCode();
                Html = httpException.GetHtmlErrorMessage();
            }

            if(data != null)
            {
                var route = data.Route as Route;
                if(route != null) RouteUrl = route.Url;

                var values = data.Values;

                Action = ToTitle((string) values["action"]);
                Controller = ToTitle((string) values["controller"]);

                RouteData = (Dictionary) values;
                DataTokens = (Dictionary) data.DataTokens;

                if(DataTokens.ContainsKey("area")) Area = ToTitle(DataTokens["area"]);
            }

            if(context == null)
            {
                Cookies =
                Form =
                QueryString =
                ServerVariables = empty;
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

        static string ToTitle(string value)
        {
            return value == null ? null : Regex.Replace(value, @"^[a-z]", m => m.Value.ToUpper());
        }

        public string Action { get; set; }
        public string Area { get; set; }
        public int? Code { get; set; }
        public string Controller { get; set; }
        public Dictionary Cookies { get; set; }
        public Dictionary DataTokens { get; set; }
        public Dictionary Form { get; set; }
        public string Html { get; set; }
        public string HttpMethod { get; set; }
        public Dictionary QueryString { get; set; }
        public Dictionary RouteData { get; set; }
        public string RouteUrl { get; set; }
        public Dictionary ServerVariables { get; set; }
        public Uri Url { get; set; }

        static readonly Dictionary empty = new Dictionary();
    }
}