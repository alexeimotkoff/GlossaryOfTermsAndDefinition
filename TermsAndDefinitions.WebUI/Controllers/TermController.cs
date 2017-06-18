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
using TermsAndDefinitions.WebUI.Filters;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;
using WebMatrix.WebData;
using DBContext = TermsAndDefinitions.WebUI.Models.GlossaryProjectDatabaseEntities;

namespace TermsAndDefinitions.WebUI.Controllers
{
    [InitializeSimpleMembership]
    public class TermController : Controller
    {
        // GET: Term

        [OutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        [AllowAnonymous]
        public ActionResult Index(int? id)
        {
            using (var db = new DBContext())
            {

                if (id == null)
                {
                    var termsColection = db.Terms;
                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                        .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                        .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                        cfg.CreateMap<Definition, DefinitionViewModel>();
                        cfg.CreateMap<Term, PreviewTermViewModel>()
                         .ForMember("Definition", opt => opt.MapFrom(c => c.Definitions
                         .OrderByDescending(d => d.Projects.Count())
                         .ThenBy(x => x.Time).FirstOrDefault()));
                    });

                    var resultTermsColection = Mapper.Map<IEnumerable<Term>, IEnumerable<PreviewTermViewModel>>(termsColection);

                    if (Request.IsAjaxRequest())
                        return PartialView("PreviewTermsPartical", resultTermsColection);

                    return View("IndexPreviewTerms", resultTermsColection);
                }
                else
                {
                    var term = db.Terms.Find(id);
                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                        .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                        .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                        cfg.CreateMap<Definition, DefinitionViewModel>()
                        .ForMember("TermName", opt => opt.MapFrom(c => c.Term.TermName))                   
                        .ForMember("Freq", opt => opt.MapFrom(c => c.Projects.Count));
                        cfg.CreateMap<Project, PreviewProjectViewModel>();
                        cfg.CreateMap<Term, TermViewModel>()
                        .ForMember("FundamentalArea", opt => opt.MapFrom(c => c.FundamentalArea.NameFundamentalArea))
                        .ForMember("IdFundamentalArea", opt => opt.MapFrom(c => c.FundamentalArea.IdFundamentalArea));
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
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditDefinition(int id)
        {
            using (var db = new DBContext())
            {
                Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Definition, DefinitionViewModel>()
                .ForMember("TermName", opt => opt.MapFrom(c => c.Term.TermName));
            });

                Definition item = db.Definitions.Find(id);
                var resultItem = Mapper.Map<Definition, DefinitionViewModel>(item);
                return PartialView("EditDefinition", resultItem);
            }
        }

        
        [HttpPost]
        public ActionResult EditAddition(int id)
        {
            using (var db = new DBContext())
            {
                ViewData["Areas"] = db.FundamentalAreas.Select(x => x.NameFundamentalArea).ToList();
                var term = db.Terms.Find(id);
                var data = new TermAdditionViewModel()
                {
                    Addition = term.Addition,
                    TermName = term.TermName,
                    IdTerm = term.IdTerm,
                    FundamentalArea = term.FundamentalArea.NameFundamentalArea,
                    IdFundamentalArea = term.FundamentalArea.IdFundamentalArea
                };
             return PartialView("EditAdditionPartical",data);
            }
        }

        [HttpPost]
        public ActionResult UpdateAddition(TermAdditionViewModel model)
        {
            using (var db = new DBContext())
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<DefinitionViewModel, Definition>();
                });

                var term = db.Terms.Find(model.IdTerm);
                term.Addition = model.Addition;
                var area = db.FundamentalAreas.FirstOrDefault(x=> x.NameFundamentalArea == model.FundamentalArea);
                term.IdFundamentalArea = area.IdFundamentalArea;
                    db.Entry(term).State = EntityState.Modified;
                db.SaveChanges();
            }
                return PartialView("AdditionPartical", model);
        }


        [HttpPost]
        public ActionResult AddDefinitionForm(int id)
        {
            using (var db = new DBContext())
            {
                var term = db.Terms.Find(id);
                var model = new DefinitionViewModel() { IdTerm = term.IdTerm, TermName = term.TermName };
                return PartialView("AddDefinition", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDefinition(DefinitionViewModel model)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Definition, DefinitionViewModel>()
                        .ForMember("TermName", opt => opt.MapFrom(c => c.Term.TermName))
                        .ForMember("Freq", opt => opt.MapFrom(c => c.Projects.Count));
                cfg.CreateMap<DefinitionViewModel, Definition>();
            });

            using (var db = new DBContext())
            {
                var data = Mapper.Map<DefinitionViewModel, Definition>(model);
                db.Definitions.Add(data);
                db.SaveChanges();
                var term = db.Terms.Find(data.IdTerm);
                var definitions = Mapper.Map<IEnumerable<Definition>, IEnumerable<DefinitionViewModel>>(term.Definitions).OrderByDescending(x => x.Freq).Skip(1);
                return PartialView("OtherDefinitionsPartical", definitions);
            }
        }

        [HttpPost, ActionName("Update")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDefinition(DefinitionViewModel model)
        {
            if (model != null)
            {
                using (var db = new DBContext())
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<DefinitionViewModel, Definition>();
                    });

                    var data = Mapper.Map<DefinitionViewModel, Definition>(model);

                    if (data.IdDefinition > 0)
                    {
                        db.Entry(data).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return PartialView("DefinitionPartical", model);
                }
            }
            return null;
        }

        [HttpGet]
        public ActionResult Add()
        {
            using (var db = new DBContext())
            {
                ViewData["Areas"] = db.FundamentalAreas.Select(x => x.NameFundamentalArea).ToList();
            }
            if (Request.IsAjaxRequest())
                return PartialView("AddPartical", new AddTermViewModel());

            return View("IndexAdd", new AddTermViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddTermViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DBContext())
                {
                    if (db.Terms.FirstOrDefault(x => x.TermName.ToLower() == model.TermName.ToLower()) == null)
                    {
                        FundamentalArea area = db.FundamentalAreas
                            .FirstOrDefault(x => x.NameFundamentalArea.ToLower() == model.FundamentalArea.ToLower());

                        if(area == null)
                        {
                            area = new FundamentalArea() { NameFundamentalArea = model.FundamentalArea };
                            db.FundamentalAreas.Add(area);
                            db.SaveChanges();
                        }
                        
                        var term = new Term()
                        {
                            TermName = model.TermName,
                            Addition = model.Addition,
                            IdFundamentalArea = area.IdFundamentalArea
                        };

                        Mapper.Initialize(cfg =>
                        {
                            cfg.CreateMap<Definition, DefinitionViewModel>()
                                    .ForMember("TermName", opt => opt.MapFrom(c => c.Term.TermName))
                                    .ForMember("Freq", opt => opt.MapFrom(c => c.Projects.Count));
                            cfg.CreateMap<DefinitionViewModel, Definition>();
                        });
                        db.Terms.Add(term);
                        db.SaveChanges();
                        var definition = new Definition()
                        {
                            IdTerm = term.IdTerm,
                            Description = model.Definition,
                            URL = model.URL,
                            URLTitle = model.URLTitle,
                            UserId = WebSecurity.CurrentUserId
                        };
                        db.Definitions.Add(definition);
                        db.SaveChanges();
                        return View("Index", term.IdTerm);
                    }
                    else
                    {
                        ModelState.AddModelError("TermName", "Данный термин уже добавлен");
                    }
                }
            }

            using (var db = new DBContext())
            {
                ViewData["Areas"] = db.FundamentalAreas.Select(x => x.NameFundamentalArea).ToList();
            }

            if (Request.IsAjaxRequest())
                return PartialView("AddPartical", new AddTermViewModel());

            return View("IndexAdd", new AddTermViewModel());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Search(string queryString)
        {
            queryString = HttpUtility.UrlDecode(queryString).ToLower();
            var searchQuery = queryString.Split(@" /+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Definition, DefinitionViewModel>();
                cfg.CreateMap<Term, PreviewTermViewModel>()
                 .ForMember("Definition", opt => opt.MapFrom(c => c.Definitions
                .OrderByDescending(d => d.Projects.Count())
                .ThenBy(x => x.Time).FirstOrDefault()));
            });

            List<Term> searchResult = new List<Term>();
            string firstCharacter = "";
            bool findByAlphabet = searchQuery.Length == 1 && searchQuery[0].Length == 1;
            if (findByAlphabet)
                firstCharacter = searchQuery[0].ToLower();
            using (var db = new DBContext())
            {
                if (findByAlphabet)
                {
                    searchResult = db.Terms.Where(x => x.TermName.ToLower().StartsWith(firstCharacter)).ToList();
                }
                else
                {
                    List<Definition> searchByDefinitions = new List<Definition>(db.Definitions);
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
        }


        #region From another controller
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PreviewTermsPartical(IEnumerable<PreviewTermViewModel> terms)
        {
            ViewData["anotherTitle"] = "Глоссарий проекта";
            return PartialView("PreviewTermsPartical", terms);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TermPartical(ProjectViewModel term)
        {
            return PartialView("TermPartical", term);
        }
    }
    #endregion
}