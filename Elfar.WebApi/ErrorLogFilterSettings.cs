using System;
using System.Web.Http.Filters;

namespace Elfar.WebApi
{
    public class ErrorLogFilterSettings
    {
        public Predicate<HttpActionExecutedContext> Exclude { get; set; }
    }
}