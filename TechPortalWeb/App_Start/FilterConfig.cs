﻿using System.Web;
using System.Web.Mvc;
using TechPortalWeb.Helpers;

namespace TechPortalWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalExceptionFilter());
            filters.Add(new HandleErrorAttribute());            
        }
    }
}
