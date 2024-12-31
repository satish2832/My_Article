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
        IList<CandidateEnquiry> GetAll();
        CandidateEnquiry GetById(Guid id);
        IList<CandidateEnquiryFollowup> GetByCandidateEnquiryId(Guid id);
        CandidateEnquiryFollowup SaveFollowup(CandidateEnquiryFollowup candidateEnquiryFollowup);
    }

    public class EnquiryService : IEnquiryService
    {
        public EnquiryService(TechPortalEntities techPortalEntities)
        {
            TechPortalEntities = techPortalEntities;
        }

        public TechPortalEntities TechPortalEntities { get; }

        public IList<CandidateEnquiry> GetAll()
        {
            return this.TechPortalEntities.CandidateEnquiries.OrderByDescending(x => x.UpdateDT).ToList();
        }

        public IList<CandidateEnquiryFollowup> GetByCandidateEnquiryId(Guid id)
        {
            return this.TechPortalEntities.CandidateEnquiryFollowups.Where(x => x.CandidateEnquiryId == id)
                                            .OrderByDescending(x => x.UpdateDT).ToList();
        }

        public CandidateEnquiry GetById(Guid id)
        {
            var candidateEnquiry = this.TechPortalEntities.CandidateEnquiries.FirstOrDefault(x => x.Id == id);
            candidateEnquiry.CandidateEnquiryFollowups = GetByCandidateEnquiryId(id);
            return candidateEnquiry;
        }

        public bool Save(CandidateEnquiry candidateEnquiry)
        {
            try
            {
                this.TechPortalEntities.CandidateEnquiries.Add(candidateEnquiry);
                this.TechPortalEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CandidateEnquiryFollowup SaveFollowup(CandidateEnquiryFollowup candidateEnquiryFollowup)
        {
            try
            {
                this.TechPortalEntities.CandidateEnquiryFollowups.Add(candidateEnquiryFollowup);
                this.TechPortalEntities.SaveChanges();
                return candidateEnquiryFollowup;
            }
            catch
            {
                throw;
            }
        }
    }
}
