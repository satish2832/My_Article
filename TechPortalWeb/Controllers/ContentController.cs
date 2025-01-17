using AppRepository;
using AppRepository.Enquiry;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TechPortalWeb.Helpers;
using TechPortalWeb.Models;

namespace TechPortalWeb.Controllers
{    
    public class ContentController : BaseController
    {
        public ContentController(IArticleService articleService)
        {
            ArticleService = articleService;
        }

        public IArticleService ArticleService { get; }

        [Route("article/{title}")]
        public ActionResult GetContent(string title)
        {
            var article = ArticleService.GetByTitle(title);           
            var content = ArticleCreateHelper.ReadContentFromGzipFile(article.ContentFileURL);
            article.ContentText = content;
            var articleModel = MapperHelper.Map<Article, ArticleModel>(article);
            var articles = ArticleService.GetAll();

            ViewBag.RelatedArticles = articles.Select(x => MapperHelper.Map<Article, ArticleModel>(x));
            return View("Content", articleModel);
        }
    }
}
