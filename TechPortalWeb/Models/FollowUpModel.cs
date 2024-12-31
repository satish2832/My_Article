using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechPortalWeb.Models
{
    public class FollowUpModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid CandidateEnquiryId { get; set; }
        public string LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedOn { get; set; }
    }
}