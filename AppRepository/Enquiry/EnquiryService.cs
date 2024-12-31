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

        public CandidateEnquiry GetById(Guid id)
        {
            return this.TechPortalEntities.CandidateEnquiries.FirstOrDefault(x => x.Id == id);
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
    }
}
