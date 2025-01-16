using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechPortalWeb.Helpers
{
    public class AjaxOrFullViewAttribute : ActionFilterAttribute
    {
        private readonly string _partialViewName;
        private readonly string _fullViewName;

        public AjaxOrFullViewAttribute(string partialViewName, string fullViewName)
        {
            _partialViewName = partialViewName;
            _fullViewName = fullViewName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (request.IsAjaxRequest())
            {
                // Return a partial view
                if (!string.IsNullOrEmpty(_partialViewName))
                {
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = _partialViewName,
                        ViewData = filterContext.Controller.ViewData,
                        TempData = filterContext.Controller.TempData
                    };
                }
            }
            else
            {
                // Return the full layout view
                if (!string.IsNullOrEmpty(_fullViewName))
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = _fullViewName,
                        ViewData = filterContext.Controller.ViewData,
                        TempData = filterContext.Controller.TempData
                    };
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

}