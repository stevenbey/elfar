using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elfar.Mvc
{
    using dictionary = IDictionary<string, string>;

    class Json : Elfar.Json
    {
        public Json(Exception exception, RouteData routeData, HttpContextBase context) : base(exception)
        {
            var httpException = exception as HttpException;

            if(httpException != null)
            {
                Code = httpException.GetHttpCode();
                Html = httpException.GetHtmlErrorMessage();
            }

            try { Host = context.Server.MachineName; }
            catch(HttpException) { }

            var route = routeData.Route as Route;
            if(route != null) RouteUrl = route.Url;

            var values = routeData.Values;

            Action = ToTitle((string) values["action"]);
            Controller = ToTitle((string) values["controller"]);

            RouteData = (Dictionary) values;
            DataTokens = (Dictionary) routeData.DataTokens;

            Area = ToTitle(DataTokens["area"]);

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
        
        public static implicit operator Json(string json)
        {
            return Serializer.Deserialize<Json>(json);
        }

        static string ToTitle(string value)
        {
            return value == null ? null : Regex.Replace(value, @"^[a-z]", m => m.Value.ToUpper());
        }

        public string Action { get; set; }
        public string Area { get; set; }
        public int? Code { get; set; }
        public string Controller { get; set; }
        public dictionary Cookies { get; set; }
        public dictionary DataTokens { get; set; }
        public dictionary Form { get; set; }
        public string Html { get; set; }
        public string HttpMethod { get; set; }
        public dictionary QueryString { get; set; }
        public dictionary RouteData { get; set; }
        public string RouteUrl { get; set; }
        public dictionary ServerVariables { get; set; }
        public Uri Url { get; set; }
    }
}