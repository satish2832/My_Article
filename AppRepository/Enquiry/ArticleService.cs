using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Enquiry
{
    public interface IArticleService
    {
        void Save(Article article);
        Article GetById(Guid id);
        Article GetByTitle(string title);
        IList<Article> GetAll();
    }
    public class ArticleService : IArticleService
    {
        private readonly TechPortalEntities techPortalEntities;

        public ArticleService(TechPortalEntities techPortalEntities)
        {
            this.techPortalEntities = techPortalEntities;
        }
        public void Save(Article article)
        {
            this.techPortalEntities.Articles.Add(article);
            this.techPortalEntities.SaveChanges();
        }

        public Article GetById(Guid id)
        {
            return this.techPortalEntities.Articles.SingleOrDefault(x => x.Id == id);
        }

        public Article GetByTitle(string title)
        {
            return this.techPortalEntities.Articles.SingleOrDefault(x => x.Title == title);
        }

        public IList<Article> GetAll()
        {
            return this.techPortalEntities.Articles.ToList();
        }
    }
}
