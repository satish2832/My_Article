using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Enquiry
{
    public interface IEnquiryService
    {
        bool Save(CandidateEnquiry candidateEnquiry);
    }

    public class EnquiryService : IEnquiryService
    {
        public EnquiryService(TechPortalEntities techPortalEntities)
        {
            TechPortalEntities = techPortalEntities;
        }

        public TechPortalEntities TechPortalEntities { get; }

        public bool Save(CandidateEnquiry candidateEnquiry)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
