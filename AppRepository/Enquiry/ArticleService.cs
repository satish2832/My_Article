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
        bool Delete(Guid id);
        IList<Article> GetAllByType(string type,bool isContains);
        IList<ArticleType> GetAllType();
        void UpdateType(ArticleType type);
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
            var articleExisted = this.techPortalEntities.Articles.SingleOrDefault(x => x.Id == article.Id);
            if (articleExisted == null)
            {
                this.techPortalEntities.Articles.Add(article);
            }
            else
            {
                if (article.removedImageIds != null)
                {
                    var removedImages = this.techPortalEntities.ArticleImages.Where(x => article.removedImageIds.Contains(x.Id)).ToList();
                    this.techPortalEntities.ArticleImages.RemoveRange(removedImages);
                }
                articleExisted.Title = article.Title;
                articleExisted.ArticleTypeId = article.ArticleTypeId;
                articleExisted.ContentFile = article.ContentFile;
                articleExisted.Tags = article.Tags;
                articleExisted.ArticleImages = article.ArticleImages;
                articleExisted.UpdateDT = article.UpdateDT;
            }
            this.techPortalEntities.SaveChanges();
        }

        public Article GetById(Guid id)
        {
            return this.techPortalEntities.Articles.SingleOrDefault(x => x.Id == id);
        }

        public Article GetByTitle(string title)
        {
            return this.techPortalEntities.Articles.SingleOrDefault(x => x.TitleURL == title);
        }

        public IList<Article> GetAll()
        {
            return this.techPortalEntities.Articles.OrderByDescending(x => x.UpdateDT).ToList();
        }

        public IList<ArticleType> GetAllType()
        {
            return this.techPortalEntities.ArticleTypes.ToList();
        }

        public void UpdateType(ArticleType type)
        {
            if (type.Id != Guid.Empty)
            {
                var artileType = this.techPortalEntities.ArticleTypes.SingleOrDefault(x => x.Id == type.Id);
                artileType.Name = type.Name;
            }
            else
            {
                type.Id = Guid.NewGuid();
                this.techPortalEntities.ArticleTypes.Add(type);
            }
            this.techPortalEntities.SaveChanges();
        }

        public IList<Article> GetAllByType(string type,bool isContains)
        {
            if (isContains)
            {
                return this.techPortalEntities.Articles.Where(x => x.ArticleType.Name.Contains(type)).OrderByDescending(x => x.UpdateDT).ToList();
            }
            else
            {
                return this.techPortalEntities.Articles.Where(x => !x.ArticleType.Name.Contains(type)).OrderByDescending(x => x.UpdateDT).ToList();
            }
        }

        public bool Delete(Guid id)
        {
            var articleImages = this.techPortalEntities.ArticleImages.Where(x => x.ArticleId == id);
            if (articleImages.Any())
            {
                this.techPortalEntities.ArticleImages.RemoveRange(articleImages);
            }
            var articleExisted = this.techPortalEntities.Articles.SingleOrDefault(x => x.Id == id);
            if (articleExisted != null)
            {
                this.techPortalEntities.Articles.Remove(articleExisted);
            }            
            this.techPortalEntities.SaveChanges();
            return true;
        }
    }
}
