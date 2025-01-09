using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechPortalWeb.Models
{
    public class ArticleCreateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public string Content { get; set; }
        public HttpPostedFile[] Images { get; set; }
        public Guid ArticleTypeId { get; set; }
        public string Tags { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}