﻿using System;
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
            this.techPortalEntities.Articles.Add(article);            
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
            return this.techPortalEntities.Articles.ToList();
        }

        public IList<ArticleType> GetAllType()
        {
            return this.techPortalEntities.ArticleTypes.ToList();
        }

        public void UpdateType(ArticleType type)
        {
            if(type.Id != Guid.Empty)
            {
                var artileType= this.techPortalEntities.ArticleTypes.SingleOrDefault(x => x.Id == type.Id); 
                artileType.Name = type.Name;                
            }
            else
            {
                type.Id = Guid.NewGuid();
                this.techPortalEntities.ArticleTypes.Add(type);
            }
            this.techPortalEntities.SaveChanges();
        }
    }
}
