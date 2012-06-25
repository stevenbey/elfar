using System.Text;
using System.Web.Mvc;

namespace Elfar.ActionResults
{
    public class CsvResult
            : ActionResult
    {
        public override void ExecuteResult(
                ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrWhiteSpace(ContentType) ? "text/csv; header=present" : ContentType;
            response.AppendHeader("Content-Disposition", "attachment; filename=elfar.csv");
            if(ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if(Data != null)
                response.Write(Data.ToString());
        }

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
    }
}