using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TechPortalWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(name: "privacy-policy", url: "privacy-policy",
                 defaults: new { controller = "Home", action = "PrivacyPolicy", name = UrlParameter.Optional });
            routes.MapRoute(name: "terms-conditions", url: "terms-conditions",
                 defaults: new { controller = "Home", action = "TermsAndConditions", name = UrlParameter.Optional });
            routes.MapRoute(name: "course-details", url: "course/{name}",
                  defaults: new { controller = "Home", action = "CourseDetails", name = UrlParameter.Optional });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
