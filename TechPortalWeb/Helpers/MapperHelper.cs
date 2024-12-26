using AppRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechPortalWeb.Models;

namespace TechPortalWeb.Helpers
{
    public static class MapperHelper
    {
        public static Destination Map<Source, Destination>(Source source, Destination destination = default(Destination))
            where Source : class
            where Destination : class, new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) destination = new Destination();

            if (source is EnquiryFormModel && destination is CandidateEnquiry)
            {
                return ConvertToCandidateEnquiry(source as EnquiryFormModel, destination as CandidateEnquiry) as Destination;
            }

            throw new InvalidOperationException($"Mapping from {typeof(Source)} to {typeof(Destination)} is not supported.");
        }

        private static CandidateEnquiry ConvertToCandidateEnquiry(EnquiryFormModel enquiryFormModel, CandidateEnquiry candidateEnquiry)
        {
            if (enquiryFormModel == null) throw new ArgumentNullException(nameof(enquiryFormModel));
            if (candidateEnquiry == null) candidateEnquiry = new CandidateEnquiry();

            candidateEnquiry.Name = enquiryFormModel.Name;
            candidateEnquiry.PhoneNumber = enquiryFormModel.PhoneNumber;
            candidateEnquiry.Email = enquiryFormModel.Email;
            candidateEnquiry.Comments = enquiryFormModel.Comments;

            return candidateEnquiry;
        }
    }

}