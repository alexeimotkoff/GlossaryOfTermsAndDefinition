using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.Models;

namespace TermsAndDefinitions.WebUI.Controllers
{
    public class SearchTermController : Controller
    {
        //
        // GET: /SearchTerm/
        projects_dataBaseEntities2 db = new projects_dataBaseEntities2();
        public ActionResult index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult search(string qery, string type )
        {
            return View();
        }

        public ActionResult getTerms(string qery, int? n)
        {

            return View();
        }

         public ActionResult getTermsByDef(string qery, int? n)
        {

            return View();
        }

    }
}
