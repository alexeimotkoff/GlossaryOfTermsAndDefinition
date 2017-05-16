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
            IQueryable<Term> searchResult = db.Terms.Where(x => x.TermName.Contains(searchQuery.querySearch)).OrderBy(x => x.TermName);
            int takeCount = Math.Min(searchQuery.countSearchItem, searchResult.Count());
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);

           List<VTerm> searchResultToViewModel = new List<VTerm>();
            foreach (var term in searchResult)
                searchResultToViewModel.Add(new VTerm(term));            
            return View(searchResultToViewModel);
        }

        public ActionResult GetTermsByDefinition(SearchQuery searchQuery)
        {
            IQueryable<Definition> searchResult = db.Definitions.Where(x => x.Description.Contains(searchQuery.querySearch)).OrderBy(x => x.Term.TermName);
            int takeCount = Math.Min(searchQuery.countSearchItem, searchResult.Count());
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
            List<VTerm> searchResultToViewModel = new List<VTerm>();
            foreach (var item in searchResult)
                searchResultToViewModel.Add(new VTerm(item.Term.TermName) { Description = new VDefinition(item)});
            return View(searchResultToViewModel);
        }

        public ActionResult GetProjects(SearchQuery searchQuery)
        {
            IQueryable<Project> searchResult = db.Projects.Where(x => x.ProjectName.Contains(searchQuery.querySearch)).OrderBy(x => x.ProjectName);
            int takeCount = Math.Min(searchQuery.countSearchItem, searchResult.Count());
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
            List<VProject> searchResultToViewModel = new List<VProject>();
            foreach (var project in searchResult) searchResultToViewModel.Add(new VProject(project));
            
            return View(searchResultToViewModel);
        }
    }
}
