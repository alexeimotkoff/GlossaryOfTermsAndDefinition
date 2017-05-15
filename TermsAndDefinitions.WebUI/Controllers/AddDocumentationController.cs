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

        [HttpPost]
        public ActionResult AddDocumetation(Project project)
        {
            
            return View();
        }

        //[HttpPost]
        //public bool AddGlossary(List<KeyValuePair<Term, Definition>> glossary)
        //{
        //    foreach (var pair in glossary)
        //    {
        //        Term term = db.Terms.FirstOrDefault(x => x.TermName.ToLower() == x.TermName.ToLower().Trim());
        //        if (term == null)
        //        {
        //            term = pair.Key;
        //            db.Terms.Add(term);
        //            db.SaveChanges();
        //        }
        //        Definition definition = new Definition() { Description = description, URL = url, IdTerm = term.IdTerm };

        //    }

            
        //}

       
        //public ActionResult AddTerm(string nameTerm, string description, string url)
        //{
           
        //}

    }
}
