using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Elfar
{
    public class ErrorLog : IXmlSerializable
    {
        public ErrorLog() { }
        public ErrorLog(string application, Exception exception) : this(application, new Json(exception)) { }
        public ErrorLog(string application, Json json)
        {
            Application = application;
            ID = json.ID;
            Json = json;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {
            ID = int.Parse(reader.GetAttribute("id"));
            Application = reader.GetAttribute("application");
            Json = reader.ReadElementContentAsString();
        }
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", ID.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("application", Application);
            writer.WriteString(Json);
        }

        public string Application { get; set; }
        public int ID { get; set; }
        public string Json { get; set; }
    }
}