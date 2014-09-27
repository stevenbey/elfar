using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Elfar.IO;

namespace Elfar.Xml
{
    [DisplayName("XML")]
    public sealed class XmlErrorLogProvider : FileErrorLogProvider, IStorageProvider
    {
        public override void Delete(int id)
        {
            lock (Key)
            {
                DocumentElement.RemoveChild(FindNode(id));
                document.Save(FilePath);
            }
        }
        public new IEnumerator<ErrorLog.Storage> GetEnumerator()
        {
            return DocumentElement.ChildNodes.Cast<XmlNode>().Select(n => (errorLog)serializer.Deserialize(new XmlNodeReader(n))).GetEnumerator();
        }
        public override void Save(ErrorLog errorLog)
        {
            lock (Key)
            {
                DocumentElement.AppendChild(CreateNode(errorLog));
                document.Save(FilePath);
            }
        }

        protected override string GetDefaultFilePath()
        {
            return defaultFilePath;
        }

        static XmlNode CreateNode(ErrorLog errorLog)
        {
            var builder = new StringBuilder();
            serializer.Serialize(XmlWriter.Create(builder), new errorLog(errorLog));
            var element = document.CreateElement("temp");
            element.InnerXml = builder.ToString();
            return element.SelectSingleNode("errorLog");
        }
        static XmlNode FindNode(int id)
        {
            return document.SelectSingleNode(string.Format("errorLogs/errorLog[@id='{0}']", id));
        }

        XmlElement DocumentElement
        {
            get
            {
                if (document.DocumentElement == null)
                {
                    if (File.Exists(FilePath))
                    {
                        document.Load(FilePath);
                    }
                    else
                    {
                        document.LoadXml(markup);
                        document.Save(FilePath);
                    }
                }
                return document.DocumentElement;
            }
        }

        const string defaultFilePath = "|DataDirectory|Elfar.xml";
        const string markup = @"<?xml version=""1.0"" encoding=""utf-8""?><errorLogs></errorLogs>";

        static readonly XmlDocument document = new XmlDocument();
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(errorLog));

        // ReSharper disable once InconsistentNaming
        public class errorLog : ErrorLog.Storage, IXmlSerializable
        {
            public errorLog() { }

            internal errorLog(ErrorLog errorLog) : base(errorLog) { }

            public XmlSchema GetSchema()
            {
                return null;
            }
            public void ReadXml(XmlReader reader)
            {
                ID = int.Parse(reader.GetAttribute("id"), CultureInfo.InvariantCulture);
                Value = reader.ReadElementContentAsString().Decompress();
            }
            public void WriteXml(XmlWriter writer)
            {
                writer.WriteAttributeString("id", ID.ToString(CultureInfo.InvariantCulture));
                writer.WriteString(Value.Compress());
            }
        }
    }
}