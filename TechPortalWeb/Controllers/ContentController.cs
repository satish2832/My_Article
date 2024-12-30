using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TechPortalWeb.Models;

namespace TechPortalWeb.Controllers
{
    [RoutePrefix("api/content")]
    public class ContentController : BaseController
    {
        [HttpGet]
        [Route("read")]
        public ActionResult Get()
        {
            var response = new HttpResponseMessage();
            return Content("Test");
        }

        [HttpPost]
        [Route("save")]
        public ActionResult Post(ContentModel contentModel)
        {
            var content = HttpUtility.UrlDecode(contentModel.Text);
            var fileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            var res = GenerateGzipFile(content, fileName);
            var response = new HttpResponseMessage();
            return Content("Data received successfully");
        }

        private bool GenerateGzipFile(string content,string fileName)
        {            
            string filePath = Server.MapPath($"~/Pages/{fileName}.txt.gz");       
                      
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
