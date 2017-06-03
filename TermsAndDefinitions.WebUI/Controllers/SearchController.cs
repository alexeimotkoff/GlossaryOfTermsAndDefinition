using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.CustomAttributes;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;

namespace TermsAndDefinitions.WebUI.Controllers
{

    [AllowAnonymous]
    public class SearchController : Controller
    {
        //
        // GET: /SearchTerm/
        //h
       GlossaryProjectDatabaseEntities db = new GlossaryProjectDatabaseEntities();
        
        public ActionResult Index(string catchall)
        {
            List<string> actions = new List<string> { "all", "terms", "definitions", "projects" };
            int actionIdx = -1;
            int count = 6;
            List<string> querys = new List<string>();
            if (!string.IsNullOrEmpty(catchall))
            {
                querys = catchall.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                actionIdx = actions.IndexOf(querys[0].ToLower()) - 1;
                if (actionIdx > -2)
                {
                    querys.RemoveAt(0);
                    count = 0;
                }
            }
            return View("Index", new SearchQuery(querys, actionIdx, count));
        }

        public ActionResult All(string query)
        {
            return PartialView("GetResultSearch",new SearchQuery(query, 0, -1));
          
        }


     

        //[AjaxOnly]
        //public ActionResult Definitions(string query)
        //{
        //    return GetResultSearch(new SearchQuery(query, 1, 0));
        //}

        //[AjaxOrChildActionOnly]
        //public ActionResult Projects(string query)
        //{
        //    return GetResultSearch(new SearchQuery(query, 2, 0));
        //}

        [ChildActionOnly]
        public ActionResult GetResultSearch(SearchQuery searchQuery)
        {
            return View(searchQuery);
        }
    
        [ChildActionOnly]
        public ActionResult GetTerms(SearchQuery searchQuery)
        {
           
            IQueryable<Term> searchResult = db.Terms.OrderBy(x => x.TermName);
            int takeCount = Math.Min(searchQuery.CountSearchItem, searchResult.Count());
                       
            foreach (var query in searchQuery.QueryToList)
                searchResult = searchResult.Where(x => x.TermName.ToLower().Contains(query));
           
            if (searchQuery.FirtstIsChar)
                searchResult = searchResult.Where(x => x.TermName.StartsWith(searchQuery.FirstQuery));
           
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
           List<VTerm> searchResultToViewModel = new List<VTerm>();
            foreach (var term in searchResult)
                searchResultToViewModel.Add(new VTerm(term));
            return View(searchResultToViewModel);
        }

        public ActionResult Terms(string query)
        {
            SearchQuery searchQuery = new SearchQuery(query, 0, 0);
            IQueryable<Term> searchResult = db.Terms.OrderBy(x => x.TermName);
            int takeCount = Math.Min(searchQuery.CountSearchItem, searchResult.Count());

            foreach (var querys in searchQuery.QueryToList)
                searchResult = searchResult.Where(x => x.TermName.ToLower().Contains(querys));

            if (searchQuery.FirtstIsChar)
                searchResult = searchResult.Where(x => x.TermName.StartsWith(searchQuery.FirstQuery));

            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
            List<VTerm> searchResultToViewModel = new List<VTerm>();
            foreach (var term in searchResult)
                searchResultToViewModel.Add(new VTerm(term));
            return PartialView("GetTerms",searchResultToViewModel);
        }

        [ChildActionOnly]
        public ActionResult GetTermsByDefinition(SearchQuery searchQuery)
        {
            IQueryable<Definition> searchResult = db.Definitions.OrderBy(x => x.Term.TermName);
            int takeCount = Math.Min(searchQuery.CountSearchItem, searchResult.Count());
            foreach (var query in searchQuery.QueryToList)
                searchResult = searchResult.Where(x => x.Description.ToLower().Contains(query));
            if (searchQuery.FirtstIsChar)
                searchResult = searchResult.Where(x => x.Description.StartsWith(searchQuery.FirstQuery));
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
            List<VTerm> searchResultToViewModel = new List<VTerm>();
            foreach (var item in searchResult.ToList())
                searchResultToViewModel.Add(new VTerm(item.Term.TermName) { Description = new VDefinition(item)});
            return View(searchResultToViewModel);
        }

        public ActionResult Definition(string query)
        {
            SearchQuery searchQuery = new SearchQuery(query, 1, 0);
            IQueryable<Definition> searchResult = db.Definitions.OrderBy(x => x.Term.TermName);
            int takeCount = Math.Min(searchQuery.CountSearchItem, searchResult.Count());
            foreach (var querys in searchQuery.QueryToList)
                searchResult = searchResult.Where(x => x.Description.ToLower().Contains(querys));
            if (searchQuery.FirtstIsChar)
                searchResult = searchResult.Where(x => x.Description.StartsWith(searchQuery.FirstQuery));
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
            List<VTerm> searchResultToViewModel = new List<VTerm>();
            foreach (var item in searchResult.ToList())
                searchResultToViewModel.Add(new VTerm(item.Term.TermName) { Description = new VDefinition(item) });
          
            return PartialView("GetTermsByDefinition", searchResultToViewModel);
        }
        [ChildActionOnly]
        public ActionResult GetProjects(SearchQuery searchQuery)
        {
            IQueryable<Project> searchResult = db.Projects.OrderBy(x => x.ProjectName);
            int takeCount = Math.Min(searchQuery.CountSearchItem, searchResult.Count());
            foreach (var query in searchQuery.QueryToList)
                searchResult = searchResult.Where(x => x.ProjectName.Contains(query));

            if (searchQuery.FirtstIsChar)
                searchResult = searchResult.Where(x =>x.ProjectName.StartsWith(searchQuery.FirstQuery));
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
            List<VProject> searchResultToViewModel = new List<VProject>();
            foreach (var project in searchResult.ToList())
                searchResultToViewModel.Add(new VProject(project));      

            return PartialView("GetProjects", searchResultToViewModel);
        }

        public ActionResult Projects(string query)
        {
            SearchQuery searchQuery = new SearchQuery(query, 2, 0);
            IQueryable<Project> searchResult = db.Projects.OrderBy(x => x.ProjectName);
            int takeCount = Math.Min(searchQuery.CountSearchItem, searchResult.Count());
            foreach (var querys in searchQuery.QueryToList)
                searchResult = searchResult.Where(x => x.ProjectName.Contains(querys));

            if (searchQuery.FirtstIsChar)
                searchResult = searchResult.Where(x => x.ProjectName.StartsWith(searchQuery.FirstQuery));
            if (takeCount > 0)
                searchResult = searchResult.Take(takeCount);
            List<VProject> searchResultToViewModel = new List<VProject>();
            foreach (var project in searchResult.ToList())
                searchResultToViewModel.Add(new VProject(project));

            return PartialView("GetProjects", searchResultToViewModel);
        }
    }
}
