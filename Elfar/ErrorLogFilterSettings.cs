using System;
using System.Web.Mvc;

namespace Elfar
{
    public class ErrorLogFilterSettings
    {
        public Predicate<ExceptionContext> Exclude { get; set; }
    }
}