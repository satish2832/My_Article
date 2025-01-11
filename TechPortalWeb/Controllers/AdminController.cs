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

        public AdminController(IEnquiryService enquiryService, ISkillsetService skillsetService)
        {
            EnquiryService = enquiryService;
            SkillsetService = skillsetService;            
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
        [Route("admin/enquiry-list")]
        public ActionResult Enquiries()
        {
            var enquiryList = EnquiryService.GetAll();
            var enquiryFormModels = enquiryList.Select(x => MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(x)).ToList();
            return PartialView(enquiryFormModels);
        }

        [AdminAuthorize]
        [Route("admin/enquiry-candidate/{id}/basic-information")]
        public ActionResult CandidateEnquiry(Guid id)
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
        public JsonResult GetEnquiryData()
        {
            var enquiryList = EnquiryService.GetAll();
            var enquiryFormModels = enquiryList.Select(x => MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(x));
            return Json(enquiryFormModels, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize]
        [Route("admin/article-create")]
        public ActionResult ArticleCreate()
        {
            return PartialView();
        }

        [AdminAuthorize]           
        public JsonResult SaveArticle(ArticleCreateModel articleCreateModel)
        {
            var content = HttpUtility.UrlDecode(articleCreateModel.Content);
            var fileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            var res = ArticleCreateHelper.GenerateGzipFile(content, fileName);
            return Json(new { },JsonRequestBehavior.AllowGet);
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