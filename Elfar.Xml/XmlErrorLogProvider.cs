using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Elfar.Xml
{
    public class XmlErrorLogProvider : IO.ErrorLogProvider
    {
        public XmlErrorLogProvider()
        {
            if(File.Exists(FilePath)) document.Load(FilePath);
            else
            {
                document.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?><errorLogs></errorLogs>");
                document.Save(FilePath);
            }
        }

        public override void Delete(int id)
        {
            lock(Key)
            {
                DocumentElement.RemoveChild(FindNode(id));
                document.Save(FilePath);
            }
        }
        public override void Save(Elfar.ErrorLog errorLog)
        {
            lock(Key)
            {
                DocumentElement.AppendChild(CreateNode(errorLog));
                document.Save(FilePath);
            }
        }

        protected override string GetDefaultFilePath()
        {
            return defaultFilePath;
        }

        static XmlNode CreateNode(Elfar.ErrorLog errorLog)
        {
            var builder = new StringBuilder();
            serializer.Serialize(XmlWriter.Create(builder), new ErrorLog(errorLog));
            var element = document.CreateElement("temp");
            element.InnerXml = builder.ToString();
            return element.SelectSingleNode("ErrorLog");
        }
        static XmlNode FindNode(int id)
        {
            return document.SelectSingleNode(string.Format("errorLogs/ErrorLog[@id='{0}']", id));
        }

        public override IEnumerable<Elfar.ErrorLog> All
        {
            get { return DocumentElement.ChildNodes.Cast<XmlNode>().Select(n => (ErrorLog) serializer.Deserialize(new XmlNodeReader(n))); }
        }

        static XmlElement DocumentElement
        {
            get
            {
                return document.DocumentElement;
            }
        }

        const string defaultFilePath = "~/App_Data/Elfar_ErrorLogs.xml";

        static readonly XmlDocument document = new XmlDocument();
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ErrorLog));
    }
}