using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Elfar.Mvc.Resources
{
    // ReSharper disable PossibleNullReferenceException
    internal class Result : FileStreamResult
    {
        static Result()
        {
            names = assembly.GetManifestResourceNames().GroupBy(n => Path.GetExtension(n).Trim('.'), Path.GetFileNameWithoutExtension);
        }

        public Result(string filename, string ext) : base(GetStream(filename, ext), "text/" + ext) { }

        static Stream GetStream(string filename, string ext)
        {
            var group = names.FirstOrDefault(g => g.Key == ext);
            if (group != null)
            {
                var resourceName = group.FirstOrDefault(f => f.EndsWith(filename, StringComparison.OrdinalIgnoreCase));
                if (resourceName != null) return assembly.GetManifestResourceStream(resourceName + "." + ext);
            }
            return null;
        }

        static readonly Assembly assembly = typeof(Result).Assembly;
        static readonly IEnumerable<IGrouping<string, string>> names;
    }
}