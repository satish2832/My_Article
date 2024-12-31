using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechPortalWeb.Models
{
    public class FollowUpModel
    {
        /// <summary>
        /// The content of the follow-up being added.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// List of previous follow-up details.
        /// </summary>
        public List<FollowUpDetail> PreviousFollowUps { get; set; }
    }

    public class FollowUpDetail
    {
        /// <summary>
        /// The name of the user who last updated this follow-up.
        /// </summary>
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// The content of the follow-up.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The timestamp when this follow-up was last updated.
        /// </summary>
        public System.DateTime LastUpdatedOn { get; set; }
    }
}