using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Elfar
{
    // ReSharper disable LoopCanBePartlyConvertedToQuery
    public sealed class Dictionary : Dictionary<string, string>
    {
        internal Dictionary(IDictionary<string, object> dictionary)
        {
            if (dictionary == null) return;
            foreach (var pair in dictionary)
            {
                var value = pair.Value as string;
                if (value != null) base.Add(pair.Key, value);
            }
        }

        Dictionary() { }
        Dictionary(NameValueCollection nvc)
        {
            if (nvc == null) return;
            foreach (string key in nvc)
            {
                base.Add(key ?? "", nvc[key]);
            }
        }

        public static implicit operator Dictionary(NameValueCollection nvc)
        {
            return new Dictionary(nvc);
        }
        public static implicit operator Dictionary(HttpCookieCollection cookies)
        {
            // This method ensures that multiple cookies, with the same name,
            // are combined into a single entry.
            var nvc = new NameValueCollection();
            if (cookies == null) return nvc;
            foreach (var name in cookies.AllKeys)
            {
                var cookie = cookies[name];
                if (cookie != null) nvc.Add(cookie.Name, cookie.Value);
            }
            return nvc;
        }

        public new void Add(string key, string value)
        {
            throw new InvalidOperationException();
        }
        public new void Clear()
        {
            throw new InvalidOperationException();
        }
        public new bool Remove(string key)
        {
            throw new InvalidOperationException();
        }

        public static Dictionary Empty = new Dictionary();
    }
}