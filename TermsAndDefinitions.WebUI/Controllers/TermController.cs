using AutoMapper;
using DotNetOpenAuth.Messaging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;
using DBContext = TermsAndDefinitions.WebUI.Models.GlossaryProjectDatabaseEntities;

namespace TermsAndDefinitions.WebUI.Controllers
{
    public class TermController : Controller
    {
        // GET: Term

        DBContext db = new DBContext();
        List<Term> Terms = new DBContext().Terms.OrderBy(t => t.TermName).ToList();
        List<Definition> Definitions = new DBContext().Definitions.OrderBy(d => d.Term.TermName).ThenBy(d => d.Frequency).ToList();

        [OutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var termsColection = Terms;
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                    .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                    .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
                cfg.CreateMap<Definition, DefinitionViewModel>();
                    cfg.CreateMap<Term, PreviewTermViewModel>().ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault()));
                });
                var resultTermsColection = Mapper.Map<IEnumerable<Term>, IEnumerable<PreviewTermViewModel>>(termsColection);

                if (Request.IsAjaxRequest())
                    return PartialView("PreviewTermsPartical", resultTermsColection);

                return View("IndexPreviewTerms", resultTermsColection);
            }else
            {
                var term = Terms.FirstOrDefault(x => x.IdTerm == id);
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                    .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                    .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                    cfg.CreateMap<Definition, DefinitionViewModel>()
                    .ForMember("TermName", opt => opt.MapFrom(c => c.Term.TermName));
                    cfg.CreateMap<Project, PreviewProjectViewModel>();
                    cfg.CreateMap<Term, TermViewModel>();
                });

                //Проекты в глоссариях которых встречается термин
                var projects = term.Definitions
                    .SelectMany(x => x.Projects)
                    .GroupBy(x => x.IdProject)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.First());

                var resultTerm = Mapper.Map<Term, TermViewModel>(term);
                resultTerm.Projects = Mapper.Map<IEnumerable<Project>, IEnumerable<PreviewProjectViewModel>>(projects);
                if (Request.IsAjaxRequest())
                    return PartialView("TermPartical", resultTerm);
                return View("IndexTerm", resultTerm);
            }
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditDefinition(int id)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Definition, DefinitionViewModel>();                
            });
            Definition item = db.Definitions.Find(id);           
            return PartialView("EditDefinition", Mapper.Map<Definition, DefinitionViewModel>(item));
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateDefinition(DefinitionViewModel model)
        {
            return Content(model.Description);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(TermViewModel term)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string queryString)
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

                searchResult = searchByDefinitions.Select(x => x.Term).Distinct().ToList();
            }

            var resultColection = Mapper.Map<IEnumerable<Term>, IEnumerable<PreviewTermViewModel>>(searchResult);

            ViewData["anotherTitle"] = "Найдено " + resultColection.Count() + " терминов";


            return PartialView("PreviewTermsPartical", resultColection);
        }


        public ContentResult UpdateField(string id, string value, int termId)
        {
            //Term term = db.Terms.Find(termId);
            //PropertyInfo propertyInfo = term.GetType().GetProperty(id);
            //propertyInfo.SetValue(term, Convert.ChangeType(value, propertyInfo.PropertyType), null);
         
            return Content(value);
        }

        #region From another controller
        [HttpGet]
        public ActionResult PreviewTermsPartical(IEnumerable<PreviewTermViewModel> terms)
        {
            ViewData["anotherTitle"] = "Глоссарий проекта";
            return PartialView("PreviewTermsPartical", terms);
        }

        //[HttpGet]
        //public ActionResult DefinitionEdit(int Id)
        //{

        //    return PartialView("DefinitionEditPartical", term);
        //}

        //[HttpPost]
        //public ActionResult DefinitionEdit(ProjectViewModel term)
        //{
        //    return PartialView("DefinitionEditPartical", term);
        //}

        [HttpGet]
        public ActionResult TermPartical(ProjectViewModel term)
        {
            return PartialView("TermPartical", term);
        }
    }
#endregion
}