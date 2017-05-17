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
            List<VTerm> terms = new List<VTerm>();
            foreach (var term in db.Terms)
                terms.Add(new VTerm(term));            
            return View(terms);
        }

    }
}
