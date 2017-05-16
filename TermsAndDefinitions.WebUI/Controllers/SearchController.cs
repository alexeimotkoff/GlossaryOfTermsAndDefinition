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
        GlossaryProjectDatabaseEntities db = new GlossaryProjectDatabaseEntities();
        public ActionResult Index()
        {            
            return View( new SearchQuery());
        }
        
        public ActionResult GetResultSearch(SearchQuery searchQuery)
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
            var resultData = searchResult.Select(x => new VTerm(x)).ToList();
            return View(resultData);
        }

        public ActionResult GetTermsByDefinition(SearchQuery searchQuery)
        {
            IEnumerable<Definition> searchResult;
            if (searchQuery.countSearchItem > 0)
                searchResult = db.Definitions
                    .Where(x => x.Description.Contains(searchQuery.querySearch)).OrderBy(x => x.Term.TermName)
                    .Take(searchQuery.countSearchItem);
            else
                searchResult = db.Definitions
                   .Where(x => x.Description.Contains(searchQuery.querySearch)).OrderBy(x => x.Term.TermName);
            List<VTerm> searchResultToViewModel = new List<VTerm>();
            foreach (var item in searchResult) searchResultToViewModel.Add(new VTerm(item.Term.TermName) { Description = new VDefinition(item)});
            
            return View(searchResultToViewModel);
        }

        public ActionResult GetProjects(SearchQuery searchQuery)
        {
            IEnumerable<Project> searchResult;
            if (searchQuery.countSearchItem > 0)
                searchResult = db.Projects.Where(x => x.ProjectName.Contains(searchQuery.querySearch))
                    .OrderBy(x => x.ProjectName).Take(searchQuery.countSearchItem);
            else
                searchResult = db.Projects.Where(x => x.ProjectName.Contains(searchQuery.querySearch))
                    .OrderBy(x => x.ProjectName);
            List<VProject> searchResultToViewModel = new List<VProject>();
            foreach (var project in searchResult) searchResultToViewModel.Add(new VProject(project));
            
            return View(searchResultToViewModel);
        }
    }
}
