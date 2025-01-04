using System.Web;
using System.Web.Mvc;

namespace TechPortalWeb.Helpers
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if the user is authenticated
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Redirect to the login page
                filterContext.Result = new RedirectResult("~/login");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}