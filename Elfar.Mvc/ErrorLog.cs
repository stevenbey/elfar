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
                Html = Regex.Replace(httpException.GetHtmlErrorMessage(), @"\s{2}", string.Empty);
            }

            if(data != null)
            {
                var route = data.Route as Route;
                if(route != null) RouteUrl = route.Url;

                var values = data.Values;

                Action = ((string) values["action"]).ToTitle();
                Controller = ((string) values["controller"]).ToTitle();

                RouteData = (Dictionary) values;
                DataTokens = (Dictionary) data.DataTokens;

                if(DataTokens.ContainsKey("area")) Area = DataTokens["area"].ToTitle();
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