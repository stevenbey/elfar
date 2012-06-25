using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Elfar.ActionResults
{
    public class XmlResult
        : ActionResult
    {
        public override void ExecuteResult(
            ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrWhiteSpace(ContentType) ? "application/xml" : ContentType;
            if(ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if(Data != null)
                using(var stream = response.OutputStream)
                    new XmlSerializer(Data.GetType()).Serialize(stream, Data);
        }

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
    }
}