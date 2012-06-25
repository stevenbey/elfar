using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Elfar
{
    public sealed class Collection
        : Dictionary<string, string>,
          IXmlSerializable
    {
        public void Add(NameValueCollection nvc)
        {
            if(nvc == null) return;
            for (var i = 0; i < nvc.Count; i++)
            {
                try
                {
                    Add(nvc.GetKey(i), nvc.Get(i));
                }
                catch(HttpRequestValidationException e)
                {
                    var parts = Regex.Replace(e.Message, @"(^.*?[(])|(?<=\=)\""|(\""\)\.$)", "").Split('=');
                    Add(parts[0], parts[1]);
                }
                catch (Exception) { }
            }
        }
        public void Add(HttpCookieCollection cookies)
        {
            if(cookies == null) return;
            foreach (var key in cookies.AllKeys)
                Add(key, cookies[key].Value);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {
            var wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if(wasEmpty) return;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var key = reader.GetAttribute("key");
                if(key != null) Add(key, reader.ReadElementContentAsString());
            }
            reader.ReadEndElement();
        }
        public override string ToString()
        {
            return serializer.Serialize(this);
        }
        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("key", key);
                writer.WriteString(this[key]);
                writer.WriteEndElement();
            }
        }

        public static implicit operator string(Collection c)
        {
            return c.ToString();
        }
        public static implicit operator Collection(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? Empty : serializer.Deserialize<Collection>(s);
        }

        static readonly Collection Empty = new Collection();

        static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
    }
}