using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Elfar
{
    static class Extensions
    {
        public static string Compress(this string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);

            using(var msi = new MemoryStream(bytes))
            using(var mso = new MemoryStream())
            {
                using(var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }

                return Convert.ToBase64String(mso.ToArray());
            }
        }
        public static string Decompress(this string value)
        {
            var bytes = Convert.FromBase64String(value);

            using(var msi = new MemoryStream(bytes))
            using(var mso = new MemoryStream())
            {
                using(var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}