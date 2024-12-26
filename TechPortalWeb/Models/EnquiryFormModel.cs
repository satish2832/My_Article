using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechPortalWeb.Models
{
    public class EnquiryFormModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Skillset { get; set; }
        public string Comments { get; set; }
    }
}