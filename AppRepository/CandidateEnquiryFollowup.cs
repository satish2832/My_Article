//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class CandidateEnquiryFollowup
    {
        public System.Guid Id { get; set; }
        public string Text { get; set; }
        public System.Guid CandidateEnquiryId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    
        public virtual CandidateEnquiry CandidateEnquiry { get; set; }
    }
}
