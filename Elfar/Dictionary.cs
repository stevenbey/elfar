using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Elfar
{
    public sealed class Dictionary : Dictionary<string, string>
    {
        public static explicit operator Dictionary(NameValueCollection nvc)
        {
            var dictionary = new Dictionary();
            if (nvc != null) foreach (var key in nvc.AllKeys) dictionary.Add(key ?? "", nvc[key]);
            return dictionary;
        }
        public static explicit operator Dictionary(HttpCookieCollection cookies)
        {
            var nvc = new NameValueCollection();
            if(cookies != null) foreach(HttpCookie cookie in cookies) nvc.Add(cookie.Name, cookie.Value);
            return (Dictionary) nvc;
        }

        public static Dictionary Empty = new Dictionary();
    }
}