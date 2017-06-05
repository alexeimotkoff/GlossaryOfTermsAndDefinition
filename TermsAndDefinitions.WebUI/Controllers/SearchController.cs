using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        
        public ActionResult Index(string queryString)
        {
            ViewData["query"] = queryString;
            return View("Index");
        }


        //async public Task<ActionResult> FundArea(string queryString)
        //{
        //    List<FundamentalArea> searchResult = new List<FundamentalArea>();
        //    var area = await db.FundamentalAreas.FirstOrDefaultAsync(x => x.NameFundamentalArea == queryString);

        //    if (area == null)
        //        searchResult = await db.FundamentalAreas.ToListAsync();
        //    else
        //        searchResult.Add(area);
            
        //    Mapper.Initialize(cfg =>
        //    {
        //        cfg.CreateMap<Term, PreviewTermViewModel>().ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault())); 
        //        cfg.CreateMap<FundamentalArea, PreviewFundArea>()
        //        .ForMember("Name", opt => opt.MapFrom(c => c.NameFundamentalArea))
        //        .ForMember("Terms", opt => opt.MapFrom(c => c.Terms));
        //    });

          
        //    var resultAreaColection = Mapper.Map<IEnumerable<FundamentalArea>, IEnumerable<PreviewFundArea>>(searchResult);
            
        //    //if (Request.IsAjaxRequest())
        //    //    return PartialView("", resultTermsColection);
        //    //return View("", resultTermsColection);
        //}


        async public Task<ActionResult> Terms(string queryString)
        {
            queryString = HttpUtility.UrlDecode(queryString);
            var searchQuery = queryString.Split(@" /+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Definition, DefinitionViewModel>();
                cfg.CreateMap<Term, PreviewTermViewModel>().ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault()));
            });

            var searchResult = await db.Terms.OrderBy(x => x.TermName).ToListAsync();
            int takeCount = Math.Min(searchQuery.Length, searchResult.Count());

            string firstCharacter = "";
            bool findByAlphabet = searchQuery.Length == 1 && searchQuery[0].Length == 1;
            if (findByAlphabet)
                firstCharacter = searchQuery[0].ToLower();

            if(findByAlphabet)
                searchResult = searchResult.Where(x => x.TermName.ToLower().StartsWith(firstCharacter)).ToList();
            else
                foreach (var query in searchQuery)
                {
                    searchResult = searchResult.Where(x => x.TermName.ToLower().Contains(query)).ToList();
                }
            
            var resultTermsColection = Mapper.Map<IEnumerable<Term>, IEnumerable<PreviewTermViewModel> >(searchResult);

            ViewData["anotherTitle"] = "Поиск терминов по запросу:  " + queryString;
            if(Request.IsAjaxRequest())
                return PartialView("~/Views/Term/PreviewTermsPartical.cshtml", resultTermsColection);
           return View("~/Views/Term/IndexTermsPartical.cshtml", resultTermsColection);
        }

        async  public Task<ActionResult> Projects(string queryString)
        {
            queryString = HttpUtility.UrlDecode(queryString);
            var searchQuery = queryString.Split(@" /+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Project, PreviewProjectViewModel>().ForMember("InformationSystem", opt => opt.MapFrom(c => c.InformationSystem.NameInformationSystem));
            });

            var searchResult = await db.Projects.OrderBy(x => x.ProjectName).ToListAsync();
            int takeCount = Math.Min(searchQuery.Length, searchResult.Count());
            string firstCharacter = "";
            bool findByAlphabet = searchQuery.Length == 1 && searchQuery[0].Length == 1;
            if (findByAlphabet)
                firstCharacter = searchQuery[0].ToLower();

            if (findByAlphabet)
                searchResult = searchResult.Where(x => x.ProjectName.StartsWith(firstCharacter)).ToList();
            else
                foreach (var query in searchQuery)
                    searchResult = searchResult.Where(x => x.ProjectName.Contains(query)).ToList();

            var resultProjectsColection = Mapper.Map<IEnumerable<Project>, IEnumerable<PreviewProjectViewModel>>(searchResult);
            if(Request.IsAjaxRequest())
                return PartialView("~/Views/Project/PreviewProjectsPartical.cshtml", resultProjectsColection);
            return View("~/Views/Project/IndexProjectsPartical.cshtml", resultProjectsColection);
        }
    }
}
