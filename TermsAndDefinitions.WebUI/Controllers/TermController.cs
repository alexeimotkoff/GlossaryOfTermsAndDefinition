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
    public class TermController : Controller
    {
        // GET: Term

        DBContext db = new DBContext();
        
        async public Task<ActionResult> IndexTerms()
        {
            var termsColection = await db.Terms.OrderBy(t=>t.TermName).ToListAsync();

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
        }

        async public Task<ActionResult> IndexTerm(string name)
        {
            var term = await db.Terms.FirstOrDefaultAsync(t => t.TermName == name);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
                cfg.CreateMap<Definition, DefinitionViewModel>();
                cfg.CreateMap<Project, PreviewProjectViewModel>();
                cfg.CreateMap<Term, TermViewModel>();
            });

            var resultTerm = Mapper.Map<Term, TermViewModel>(term);
            if (Request.IsAjaxRequest())
                return PartialView("TermPartical", resultTerm);
            return View("IndexTerm", resultTerm);
        }

        [ChildActionOnly]
        public ActionResult PreviewTermsPartical(IEnumerable<PreviewTermViewModel> terms)
        {
            ViewData["anotherTitle"] = "Глоссарий проекта";
            return PartialView("PreviewTermsPartical", terms);
        }

        [ChildActionOnly]
        public ActionResult TermPartical(ProjectViewModel term)
        {
            return PartialView("TermPartical", term);
        }
    }
}