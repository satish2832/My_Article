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

        public static string UpdateImageInContent(string[] images,string content)
        {
            if (images == null || images.Length == 0 || string.IsNullOrEmpty(content))
                return content;

            for (int i = 0; i < images.Length; i++)
            {
                // Build the placeholder based on the index
                string placeholder = $"<<image{i + 1}>>";
                string encodedPlaceholder = HttpUtility.HtmlEncode(placeholder);
                // Replace the placeholder with the <img> tag
                string imgTag = $"<img src=\"{images[i]}\" alt=\"Image{i + 1}\" class=\"img-fluid\" />";
                content = content.Replace(encodedPlaceholder, imgTag);
            }

            return content;
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

        public static byte[] ConvertImageToByteArray(HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.InputStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        public static void SaveByteArrayAsImage(byte[] imageBytes, string fileName)
        {
            // Ensure the folder path exists
            var folderPath = HttpContext.Current.Server.MapPath("~/Content/assets/img/articles/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate the full file path with the .jpg extension
            var filePath = Path.Combine(folderPath, fileName + ".jpg");

            // Write the byte array to the file
            File.WriteAllBytes(filePath, imageBytes);
        }

    }
}