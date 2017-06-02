using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.Models;

namespace TermsAndDefinitions.WebUI.Controllers
{
    public class EditTermController : Controller
    {
        //
        // GET: /EditTerm/
        //
        //GlossaryProjectDatabaseEntities db = new GlossaryProjectDatabaseEntities();
        public ActionResult Index()
        {
            return View();
        }

    }
}
