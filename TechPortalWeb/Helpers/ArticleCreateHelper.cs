﻿using System;
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

        public static byte[] GetByFileName(string fileName)
        {
            string filePath = HttpContext.Current.Server.MapPath($"~/Pages/{fileName}.txt.gz");
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public static string ReadContentFromGzipFile(string fileName)
        {
            string filePath = HttpContext.Current.Server.MapPath($"~/Pages/{fileName}.txt.gz");

            // Read compressed content from the file
            byte[] compressedBytes = System.IO.File.ReadAllBytes(filePath);

            // Decompress the content using GZipStream
            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (MemoryStream decompressedStream = new MemoryStream())
                    {
                        decompressionStream.CopyTo(decompressedStream);

                        // Convert decompressed bytes to string
                        string originalContent = Encoding.UTF8.GetString(decompressedStream.ToArray());
                        return originalContent;
                    }
                }
            }
        }
    }
}