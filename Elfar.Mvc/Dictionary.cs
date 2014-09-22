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
            if (routeValues != null) foreach (var key in routeValues.Keys) Add(nvc, key, routeValues[key]);
            return (Dictionary) nvc;
        }

        static void Add(NameValueCollection nvc, string key, object value)
        {
            var s = value as string;
            if (s == null)
            {
                var items = value as IEnumerable;
                if (items != null) foreach (var item in items) Add(nvc, key, item);
                else nvc.Add(key, value.ToString());
            }
            else nvc.Add(key, s);
        }

        public static Dictionary Empty = new Dictionary();
    }
}