using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Elfar
{
    // ReSharper disable LoopCanBePartlyConvertedToQuery
    // ReSharper disable InvertIf
    public sealed class Dictionary : Dictionary<string, string>
    {
        public Dictionary(IDictionary<string, object> dictionary)
        {
            foreach (var pair in dictionary)
            {
                var value = pair.Value as string;
                if (value != null) Add(pair.Key, value);
            }
        }

        internal Dictionary() { }

        public static explicit operator Dictionary(NameValueCollection nvc)
        {
            var dictionary = new Dictionary();
            if (nvc != null) foreach (string key in nvc) dictionary.Add(key ?? "", nvc[key]);
            return dictionary;
        }
        public static explicit operator Dictionary(HttpCookieCollection cookies)
        {
            var nvc = new NameValueCollection();
            if(cookies != null)
                foreach (string name in cookies)
                {
                    var cookie = cookies[name];
                    if (cookie != null) nvc.Add(cookie.Name, cookie.Value);
                }
            return (Dictionary) nvc;
        }

        public static Dictionary Empty = new Dictionary();
    }
}