using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechPortalWeb.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string message = "An unexpected error occurred.")
        {
            return View("Error", (object)message);
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("Error", new HandleErrorInfo(new Exception("The page you are looking for could not be found."), "Error", "NotFound"));
        }        
    }
}