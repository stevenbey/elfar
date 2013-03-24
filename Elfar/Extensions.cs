using System.IO;

namespace Elfar
{
    public static class Extensions
    {
        public static string Name(this IErrorLogPlugin plugin)
        {
            return Path.GetExtension(plugin.GetType().Namespace).Substring(1);
        }
        public static string ShortName(this string s)
        {
            return Path.GetExtension(s).Trim('.').Replace("Exception", "");
        }
    }
}