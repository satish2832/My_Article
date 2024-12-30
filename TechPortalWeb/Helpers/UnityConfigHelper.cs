using AppRepository;
using AppRepository.Enquiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace TechPortalWeb.Helpers
{
    public static class UnityConfigHelper
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<DbContext, TechPortalEntities>();
            // Register your types here
            container.RegisterType<ISkillsetService, SkillsetService>();
            container.RegisterType<IEnquiryService, EnquiryService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}