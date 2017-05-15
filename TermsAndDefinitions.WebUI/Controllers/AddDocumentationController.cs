using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.Models;

namespace TermsAndDefinitions.WebUI.Controllers
{
  
    public class AddDocumentationController : Controller
    {
        //
        // GET: /AddDocumentation/

        projects_dataBaseEntities2 db = new projects_dataBaseEntities2();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPatch]
        public ActionResult AddDocumetation(Project project)
        {
            
            return View();
        }

        public ActionResult AddGlossary

        [HttpPatch]
        public ActionResult AddTerm(string nameTerm, string description, string url)
        {
            Term term = db.Terms.FirstOrDefault(x => x.TermName.ToLower() == x.TermName.ToLower().Trim());
            if (term == null)
            {
                term = new Term() { TermName = nameTerm };
                db.Terms.Add(term);
                db.SaveChanges();
            }
            Definition definition = new Definition() { Description = description, URL = url, IdTerm = term.IdTerm };

            return View();
        }

    }
}
