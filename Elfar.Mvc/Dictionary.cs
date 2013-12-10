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
                foreach(var key in nvc.AllKeys)
                    try { dictionary.Add(key, nvc[key]); }
                    catch(HttpRequestValidationException e)
                    {
                        var parts = Regex.Replace(e.Message, @"(^.*?[(])|(?<=\=)\""|(\""\)\.$)", "").Split('=');
                        dictionary.Add(parts[0], parts[1]);
                    }
                    catch(Exception) {}
            return dictionary;
        }
        public static explicit operator Dictionary(HttpCookieCollection cookies)
        {
            var nvc = new NameValueCollection();
            if(cookies != null)
                foreach(var key in cookies.AllKeys)
                    nvc.Add(key, cookies[key].Value);
            return (Dictionary) nvc;
        }
        public static explicit operator Dictionary(RouteValueDictionary routeValues)
        {
            var nvc = new NameValueCollection();
            if(routeValues != null)
                foreach(var key in routeValues.Keys)
                {
                    var value = routeValues[key];
                    if(value is IEnumerable && !(value is string))
                        foreach(var item in (IEnumerable) value)
                            nvc.Add(key, item.ToString());
                    else
                        nvc.Add(key, routeValues[key].ToString());
                }
            return (Dictionary) nvc;
        }
    }
}