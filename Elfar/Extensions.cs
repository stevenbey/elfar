using System.IO;

namespace Elfar
{
    public static class Extensions
    {
        public static string ShortName(this string s)
        {
            return Path.GetExtension(s).Trim('.').Replace("Exception", "");
        }
    }
}