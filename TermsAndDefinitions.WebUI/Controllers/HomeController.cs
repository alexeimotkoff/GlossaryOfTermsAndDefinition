using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;
using DBContext = TermsAndDefinitions.WebUI.Models.GlossaryProjectDatabaseEntities;
namespace TermsAndDefinitions.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        //
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            //List<VTerm> terms = new List<VTerm>();
            //foreach (var term in db.Terms)
            //    terms.Add(new VTerm(term));            
           return View();
        }
    }
}
