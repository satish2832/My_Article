using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;

namespace TechPortalWeb.Helpers
{
    public class ArticleCreateHelper
    {
        public static bool GenerateGzipFile(string content, string fileName)
        {
            string filePath = HttpContext.Current.Server.MapPath($"~/Pages/{fileName}.txt.gz");

            byte[] bytes = Encoding.UTF8.GetBytes(content);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }
            }
            return true;
        }
    }
}