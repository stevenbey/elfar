using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Elfar.Mvc
{
    public sealed class Dictionary : Dictionary<string, string>
    {
        public static explicit operator Dictionary(NameValueCollection nvc)
        {
            var dictionary = new Dictionary();
            if(nvc != null)
                try { foreach(var key in nvc.AllKeys) dictionary.Add(key, nvc[key]); }
                catch(HttpRequestValidationException e)
                {
                    var parts = Regex.Replace(e.Message, @"(^.*?[(])|(?<=\=)\""|(\""\)\.$)", "").Split('=');
                    dictionary.Add(parts[0], parts[1]);
                }
                catch(Exception) { }
            return dictionary;
        }
        public static explicit operator Dictionary(HttpCookieCollection cookies)
        {
            var nvc = new NameValueCollection();
            if(cookies != null) foreach(HttpCookie cookie in cookies) nvc.Add(cookie.Name, cookie.Value);
            return (Dictionary) nvc;
        }
        public static explicit operator Dictionary(RouteValueDictionary routeValues)
        {
            var nvc = new NameValueCollection();
            if(routeValues != null) foreach(var key in routeValues.Keys) Add(key, routeValues[key], nvc);
            return (Dictionary) nvc;
        }

        static void Add(string key, object value, NameValueCollection nvc)
        {
            if(value is string) nvc.Add(key, (string) value);
            else if(value is IEnumerable) foreach(var item in (IEnumerable) value) Add(key, item, nvc);
            else nvc.Add(key, value.ToString());
        }
    }
}