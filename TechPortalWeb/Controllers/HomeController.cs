﻿using System.IO.Compression;
using System.IO;
using System.Text;
using System.Web.Mvc;
using AppRepository.Enquiry;
using TechPortalWeb.Models;
using TechPortalWeb.Helpers;
using AppRepository;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;
using System;

namespace TechPortalWeb.Controllers
{
    public class HomeController : BaseController
    {
        public IEnquiryService EnquiryService { get; }
        public ISkillsetService SkillsetService { get; }

        public HomeController(IEnquiryService enquiryService, ISkillsetService skillsetService)
        {
            EnquiryService = enquiryService;
            SkillsetService = skillsetService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult SubmitEnquiry(EnquiryFormModel enquiryFormModel)
        {
            try
            {
                var customerEnquiry = MapperHelper.Map<EnquiryFormModel, CandidateEnquiry>(enquiryFormModel);
                var isSaved = EnquiryService.Save(customerEnquiry);
                return Json(new { IsValid = isSaved }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { IsValid = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetSkillset()
        {
            var skillSets = SkillsetService.GetSkillsetList();
            var skillSetsModel = skillSets.Select(x => MapperHelper.Map<Skillset, SkillsetModel>(x));
            return Json(new { Data = skillSetsModel }, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize]
        [Route("enquiry-list")]
        public ActionResult Enquiries()
        {
            var enquiryList = EnquiryService.GetAll();
            var enquiryFormModels = enquiryList.Select(x => MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(x));
            return View(enquiryFormModels);
        }

        [AdminAuthorize]
        [Route("enquiry-candidate/{id}")]
        public ActionResult CandidateEnquiry(Guid id)
        {
            var enquiry = EnquiryService.GetById(id);
            if (enquiry == null)
            {
                return RedirectToAction("Index", "Error", new { message = "Enquiry not found." });
            }
            var enquiryModel = MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(enquiry);
            return View(enquiryModel);
        }

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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "admin" && model.Password == "password")
                {
                    // Set the authentication cookie
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    return Redirect("~/enquiry-list");
                }
                ViewData["login-error"] = "Invalid username or password.";
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }

        public JsonResult GetEnquiryData()
        {
            var enquiryList = EnquiryService.GetAll();
            var enquiryFormModels = enquiryList.Select(x => MapperHelper.Map<CandidateEnquiry, EnquiryFormModel>(x));
            return Json(enquiryFormModels, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Content()
        {
            var fileName = "60a7a1a6-c2d5-4616-867e-75327e824bb413_02_2024_08_34_23";
            var content = ReadContentFromGzipFile(fileName);
            ViewBag.htmlCotent = content;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult CourseDetails(string name)
        {
            var viewName = "DotnetCourse";
            if (name == "java-full-stack-development")
            {
                viewName = "JavaCourse";
            }
            else if (name == "dotnet-full-stack-development")
            {
                viewName = "DotnetCourse";
            }
            else if (name == "python-full-stack-development")
            {
                viewName = "PythonCourse";
            }
            else if (name == "testing-tech-stack")
            {
                viewName = "QACourse";
            }
            else if (name == "devops-tech-stack")
            {
                viewName = "DevOpsCourse";
            }
            return View(viewName);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page is here.";

            return View();
        }

        private string ReadContentFromGzipFile(string fileName)
        {
            string filePath = Server.MapPath($"~/Pages/{fileName}.txt.gz");

            // Read compressed content from the file
            byte[] compressedBytes = System.IO.File.ReadAllBytes(filePath);

            // Decompress the content using GZipStream
            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (MemoryStream decompressedStream = new MemoryStream())
                    {
                        decompressionStream.CopyTo(decompressedStream);

                        // Convert decompressed bytes to string
                        string originalContent = Encoding.UTF8.GetString(decompressedStream.ToArray());
                        return originalContent;
                    }
                }
            }
        }
    }
}