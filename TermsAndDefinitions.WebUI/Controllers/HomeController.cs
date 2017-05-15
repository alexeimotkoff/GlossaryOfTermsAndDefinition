﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;

namespace TermsAndDefinitions.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        projects_dataBaseEntities2 db = new projects_dataBaseEntities2();

        public ActionResult Index()
        {
            var terms = db.Terms.Select((x) => new VTerm(x));
            
            return View(terms);
        }

    }
}
