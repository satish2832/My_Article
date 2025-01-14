using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechPortalWeb.Models
{
    public class ArticleCreateModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TitleURL { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public IEnumerable<HttpPostedFileBase> Images { get; set; }
        public byte[] ContentFile { get; set; }
        public string ContentFileURL { get; set; }
        public Guid? ArticleTypeId { get; set; }
        public string Tags { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}