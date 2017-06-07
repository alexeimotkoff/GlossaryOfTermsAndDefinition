using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TermsAndDefinitions.WebUI.CustomAttributes;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;
using DBContext = TermsAndDefinitions.WebUI.Models.GlossaryProjectDatabaseEntities;

namespace TermsAndDefinitions.WebUI.Controllers
{

    [AllowAnonymous]
    public class SearchController : Controller
    {
        //
        // GET: /SearchTerm/
        //

        DBContext db = new DBContext();
        List<Term> Terms = new DBContext().Terms.OrderBy(t => t.TermName).ToList();
        List<Definition> Definitions = new DBContext().Definitions.OrderBy(d => d.Term.TermName).ThenBy(d => d.Frequency).ToList();
        List<Project> Projects = new DBContext().Projects.ToList();

        [OutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(string queryString)
        {
            ViewData["query"] = queryString;
            if (Request.IsAjaxRequest())
                return View("SearchPartical");
            return View("IndexSearch");
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

        [HttpGet, ActionName("Terms")]
        public ActionResult GetTerms(string queryString)
        {
            queryString = HttpUtility.UrlDecode(queryString).ToLower();
            var searchQuery = queryString.Split(@" /+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Definition, DefinitionViewModel>();
                cfg.CreateMap<Term, PreviewTermViewModel>().ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault()));
            });

            List<Term> searchResult = new List<Term>();
            string firstCharacter = "";
            bool findByAlphabet = searchQuery.Length == 1 && searchQuery[0].Length == 1;
            if (findByAlphabet)
                firstCharacter = searchQuery[0].ToLower();

            if (findByAlphabet)
            {
                searchResult = Terms.Where(x => x.TermName.ToLower().StartsWith(firstCharacter)).ToList();
            }
            else
            {
                List<Definition> searchByDefinitions = new List<Definition>(Definitions);
                foreach (var query in searchQuery)
                {
                    searchByDefinitions = searchByDefinitions.Where(x =>
                    {
                        return x.Term.TermName.ToLower().Contains(query) || x.Description.ToLower().Contains(query);
                    }).ToList();
                }
                
                searchResult = searchByDefinitions.Select( x => x.Term).Distinct().ToList();
            }

            var resultColection = Mapper.Map<IEnumerable<Term>, IEnumerable<PreviewTermViewModel>>(searchResult);

            ViewData["anotherTitle"] = "Найдено " + resultColection.Count() + " терминов";

            return PartialView("~/Views/Term/PreviewTermsPartical.cshtml", resultColection);           
        }

        [HttpGet, ActionName("Projects")]
        public ActionResult GetProjects(string queryString)
        {
            queryString = HttpUtility.UrlDecode(queryString).ToLower();
            var searchQuery = queryString.Split(@" /+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
               .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
               .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                cfg.CreateMap<Project, PreviewProjectViewModel>();
            });

            List<Project> searchResult =  new List<Project>(Projects);
            string firstCharacter = "";
            bool findByAlphabet = searchQuery.Length == 1 && searchQuery[0].Length == 1;
            if (findByAlphabet)
                firstCharacter = searchQuery[0].ToLower();

            if (findByAlphabet)
                searchResult = searchResult.Where(x => x.ProjectName.StartsWith(firstCharacter)).ToList();
            else
                foreach (var query in searchQuery)
                    searchResult = searchResult.Where(x =>
                    {
                        return x.ProjectName.ToLower().Contains(query) || x.Annotation.ToLower().Contains(query);
                    }
                    ).ToList();

            var resultColection = Mapper.Map<IEnumerable<Project>, IEnumerable<PreviewProjectViewModel>>(searchResult);

            ViewData["anotherTitle"] = "Найдено" + resultColection.Count() + "проектов";
            
            return PartialView("~/Views/Project/PreviewProjectsPartical.cshtml", resultColection);           
        }
    }
}
