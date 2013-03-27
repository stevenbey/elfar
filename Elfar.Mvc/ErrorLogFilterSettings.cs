using System;
using System.Web.Mvc;

namespace Elfar.Mvc
{
    public class ErrorLogFilterSettings
    {
        public Predicate<ExceptionContext> Exclude { get; set; }
    }
}