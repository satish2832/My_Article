using AppRepository;
using AppRepository.Enquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TechPortalWeb.Helpers;
using TechPortalWeb.Models;

namespace TechPortalWeb.Controllers
{
    public class AdminController : BaseController
    {
        public IEnquiryService EnquiryService { get; }
        public ISkillsetService SkillsetService { get; }
        public IArticleService ArticleService { get; }

        public AdminController(IEnquiryService enquiryService, ISkillsetService skillsetService, IArticleService articleService)
        {
            EnquiryService = enquiryService;
            SkillsetService = skillsetService;
            ArticleService = articleService;
        }

        [AdminAuthorize]
        [Route("admin")]
        public ActionResult Admin()
        {
            return View();
        }

        [AdminAuthorize]
        [Route("admin/candidate/{id}")]
        public ActionResult Candidate(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }

        [AdminAuthorize]
        [AjaxOrFullView("Enquiries", "Admin")]
        [Route("admin/enquiry-list")]
        public ActionResult Enquiries()
        {
            var enquiryList = EnquiryService.GetAll();
            var enquiryFormModels = enquiryList.Select(x => MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(x)).ToList();
            return PartialView(enquiryFormModels);
        }

        [AdminAuthorize]
        [Route("admin/enquiry-candidate/{id}/basic-information")]
        public ActionResult CandidateEnquiryCreate(Guid id)
        {
            var enquiry = EnquiryService.GetById(id);
            if (enquiry == null)
            {
                return RedirectToAction("Index", "Error", new { message = "Enquiry not found." });
            }
            var enquiryModel = MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(enquiry);
            return PartialView(enquiryModel);
        }

        [AdminAuthorize]
        [Route("admin/enquiry-candidate/{id}/followup-details")]
        public ActionResult CandidateFollowupDetails(Guid id)
        {
            var enquiry = EnquiryService.GetById(id);
            if (enquiry == null)
            {
                return RedirectToAction("Index", "Error", new { message = "Enquiry not found." });
            }
            var enquiryModel = MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(enquiry);
            return PartialView(enquiryModel.followUps);
        }

        [AdminAuthorize]
        public ActionResult SaveFollowup(FollowUpModel followUpModel)
        {
            try
            {
                var candidateEnquiryFollowup = MapperHelper.Map<FollowUpModel, CandidateEnquiryFollowup>(followUpModel);
                candidateEnquiryFollowup = EnquiryService.SaveFollowup(candidateEnquiryFollowup);
                followUpModel = MapperHelper.Map<CandidateEnquiryFollowup, FollowUpModel>(candidateEnquiryFollowup);
                return Json(new { IsValid = true, Data = followUpModel }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { IsValid = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [AdminAuthorize]
        [Route("admin/enquiries-all")]
        public JsonResult GetEnquiryData()
        {
            var enquiryList = EnquiryService.GetAll();
            var enquiryFormModels = enquiryList.Select(x => MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(x));
            return Json(enquiryFormModels, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize]
        [Route("admin/article-create/{id?}")]
        [AjaxOrFullView("ArticleCreate", "Admin")]
        public ActionResult ArticleCreate(Guid? id)
        {
            ArticleModel articleModel = new ArticleModel();
            var articleTypes = ArticleService.GetAllType();
            if (id == null)
            {                
                articleModel.ArticleTypes = articleTypes.Select(x => new ArticleTypeModel() { Id = x.Id, Name = x.Name });
                return PartialView(articleModel);
            }

            var article = ArticleService.GetById(id.Value);
            articleModel = MapperHelper.Map<Article, ArticleModel>(article);          
            articleModel.ArticleTypes = articleTypes.Select(x => new ArticleTypeModel() { Id = x.Id, Name = x.Name });

            articleModel.Content = ArticleCreateHelper.ReadContentFromGzipFile(articleModel.ContentFileURL);
            return PartialView(articleModel);
        }

        [AdminAuthorize]
        [AjaxOrFullView("GetAllArticle", "Admin")]
        [Route("admin/articles-all")]
        public ActionResult GetAllArticle()
        {
            var articleList = ArticleService.GetAll();
            var articleModels = articleList.Select(x => MapperHelper.Map<Article, ArticleModel>(x));
            return PartialView(articleModels);
        }

        [AdminAuthorize]
        [Route("admin/save-article")]
        public JsonResult SaveArticle(ArticleModel articleCreateModel)
        {            
            var article = MapperHelper.Map<ArticleModel, Article>(articleCreateModel);
            foreach (var articleImage in article.ArticleImages)
            {
                if (articleImage != null)
                {
                    ArticleCreateHelper.SaveByteArrayAsImage(articleImage.Image, articleImage.ImageURL);
                }
            }
            var imageUrls = article.ArticleImages.Select(x => $"/Content/assets/img/articles/{x.ImageURL}.jpg").ToArray();
            var contentWithImages = ArticleCreateHelper.UpdateImageInContent(imageUrls, articleCreateModel.Content);
            var content = HttpUtility.UrlDecode(contentWithImages);
            var fileName = articleCreateModel.Id == Guid.Empty ?
                "Article_" + Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") : articleCreateModel.ContentFileURL;
            var isFileGenerated = ArticleCreateHelper.GenerateGzipFile(content, fileName);
            if (isFileGenerated)
            {
                article.ContentFileURL = fileName;
                article.ContentFile = ArticleCreateHelper.GetByFileName(fileName);
            }
            ArticleService.Save(article);
            return Json(new { IsSaved = true }, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize]
        [AjaxOrFullView("ArticleTypeCreate", "Admin")]
        [Route("admin/article-type-create")]
        public ActionResult ArticleTypeCreate()
        {
            var articleTypes = this.ArticleService.GetAllType();
            var articleTypesModels = articleTypes.Select(x => new ArticleTypeModel() { Id = x.Id, Name = x.Name }).ToList();
            return PartialView(articleTypesModels);
        }

        [AdminAuthorize]
        [Route("admin/article-type-save")]
        public ActionResult ArticleTypeSave(ArticleTypeModel articleTypeModel)
        {
            var articleType = new ArticleType() { Id = articleTypeModel.Id, Name = articleTypeModel.Name };
            ArticleService.UpdateType(articleType);
            return Json(new { IsSaved = true, Id = articleType.Id }, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize]
        [AjaxOrFullView("JobTemplate", "Admin")]
        [Route("admin/job-template")]
        public ActionResult JobTemplate()
        {
            return PartialView();
        }

        [Route("login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginProcess(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "admin" && model.Password == "password")
                {
                    // Set the authentication cookie
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    return Redirect("~/admin");
                }
                ViewData["login-error"] = "Invalid username or password.";
            }
            return View(model);
        }        

        [AdminAuthorize]
        [Route("logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }
    }
}