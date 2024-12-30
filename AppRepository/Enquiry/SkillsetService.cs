using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Enquiry
{
    public interface ISkillsetService
    {
        IList<Skillset> GetSkillsetList();
    }

    public class SkillsetService : ISkillsetService
    {
        public SkillsetService(TechPortalEntities techPortalEntities)
        {
            TechPortalEntities = techPortalEntities;
        }

        public TechPortalEntities TechPortalEntities { get; }
        public IList<Skillset> GetSkillsetList()
        {
            return this.TechPortalEntities.Skillsets.ToList();
        }
    }
}
