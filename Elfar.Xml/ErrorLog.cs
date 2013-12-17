using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Elfar.Xml
{
    public class ErrorLog : Elfar.ErrorLog, IXmlSerializable
    {
        public ErrorLog() {}
        public ErrorLog(Elfar.ErrorLog errorLog)
        {
            ID = errorLog.ID;
            Json = errorLog.Json;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {
            ID = int.Parse(reader.GetAttribute("id"));
            Json = reader.ReadElementContentAsString().Decompress();
        }
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", ID.ToString(CultureInfo.InvariantCulture));
            writer.WriteString(Json.Compress());
        }
    }
}