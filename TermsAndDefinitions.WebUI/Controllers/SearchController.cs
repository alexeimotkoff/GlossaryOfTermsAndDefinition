using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;

namespace TermsAndDefinitions.WebUI.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /SearchTerm/
        //h
        projects_dataBaseEntities2 db = new projects_dataBaseEntities2();
        public ActionResult Index()
        {
            SearchQuery searchQuery = new SearchQuery(null, null);
            return View();
        }

        [HttpGet]
        public ActionResult Search(SearchQuery searchQuery)
        {
            return View(searchQuery);
        }

        public ActionResult GetTerms(SearchQuery searchQuery)
        {
            IEnumerable<Term> searchResult;
            if (searchQuery.countSearchItem > 0)
                searchResult = db.Terms.Where(x => x.TermName.Contains(searchQuery.querySearch)).OrderBy(x => x.TermName).Take(searchQuery.countSearchItem);
            else
                searchResult = db.Terms.Where(x => x.TermName.Contains(searchQuery.querySearch)).OrderBy(x => x.TermName);
            var resultData = searchResult.Select(x => new VTerm(x));
            return View(resultData);
        }

        public ActionResult GetTermsByDefinition(SearchQuery searchQuery)
        {
            IEnumerable<VTerm> searchResult;
            if (searchQuery.countSearchItem > 0)
                searchResult = db.Definitions
                    .Where(x => x.Description.Contains(searchQuery.querySearch)).OrderBy(x => x.Term.TermName)
                    .Take(searchQuery.countSearchItem).Select(x => new VTerm(x.Term.TermName) { Description = new VDefinition(x)});
            else
                searchResult = db.Definitions
                   .Where(x => x.Description.Contains(searchQuery.querySearch)).OrderBy(x => x.Term.TermName)
                   .Select(x => new VTerm(x.Term.TermName) { Description = new VDefinition(x)});

            return View(searchResult);
        }

    }
}
