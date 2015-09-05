using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Elfar
{
    static class Extensions
    {
        public static string ToPascal(this object value)
        {
            var s = value as string;
            return string.IsNullOrWhiteSpace(s) ? null : Regex.Replace(s, @"^[a-z]", m => m.Value.ToUpper());
        }
        public static string Compress(this string value)
        {
            using (var msi = new MemoryStream(Encoding.GetBytes(value)))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress)) msi.CopyTo(gs);
                return Convert.ToBase64String(mso.ToArray());
            }
        }
        public static string Decompress(this string value)
        {
            using (var msi = new MemoryStream(Convert.FromBase64String(value)))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) gs.CopyTo(mso);
                return Encoding.GetString(mso.ToArray());
            }
        }

        static Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}