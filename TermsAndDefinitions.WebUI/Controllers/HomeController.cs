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

        public ActionResult Terms(string name)
        {
            Term term = db.Terms.FirstOrDefault(x => x.TermName == name);
            List<Definition> definitions = term.Definitions.OrderByDescending(y => y.Frequency).ToList();
            List<VDefinition> result= new List<VDefinition>();
            foreach(var def in definitions)
            {
                result.Add(new VDefinition(def));
            }
            return View(result);
        }
        public ActionResult Terms()
        {
            List<Term> terms = db.Terms.ToList();           
            List<VTerm> result = new List<VTerm>();
            foreach (var term in terms)
            {
               result.Add(new VTerm(term));
            }
            return View(result);
        }
    }
}
