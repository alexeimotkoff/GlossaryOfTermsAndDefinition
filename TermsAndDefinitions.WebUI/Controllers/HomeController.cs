using System;
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
        //
        GlossaryProjectDatabaseEntities db = new GlossaryProjectDatabaseEntities();

        public ActionResult Index()
        {          
            return View(new SearchQuery());
        }

    }
}
