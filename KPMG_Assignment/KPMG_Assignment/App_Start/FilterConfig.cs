﻿using System.Web;
using System.Web.Mvc;

namespace KPMG_Assignment
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute(),2);
            filters.Add(new HandleErrorAttribute
            {
                View = "Error"
            }, 1);

        }
    }
}
