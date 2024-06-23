using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TechPortalWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Content()
        {
            var fileName = "60a7a1a6-c2d5-4616-867e-75327e824bb413_02_2024_08_34_23";
            var content = ReadContentFromGzipFile(fileName);
            ViewBag.htmlCotent = content;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult CourseDetails(string name)
        {
            var viewName = "DotnetCourse";
            if (name == "java-full-stack-development")
            {
                viewName = "DotnetCourse";
            }
            else if (name == "dotnet-full-stack-development")
            {
                viewName = "DotnetCourse";
            }
            return View(viewName);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page is here.";

            return View();
        }

        private string ReadContentFromGzipFile(string fileName)
        {
            string filePath = Server.MapPath($"~/Pages/{fileName}.txt.gz");

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