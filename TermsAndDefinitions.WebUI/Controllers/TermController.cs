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

        async public Task<ActionResult> IndexTerm(string name)
        {
            var term = await db.Terms.FirstOrDefaultAsync(t => t.TermName == name);
            Mapper.Initialize(cfg => cfg.CreateMap<Term, TermViewModel>()
                .ForMember("Definitions", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).Select(x => x.Description)))
                .ForMember("URLs", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).Select(x => x.URL))));
            var resultTerm = Mapper.Map<Term, TermViewModel>(term);
            if (Request.IsAjaxRequest())
                return PartialView("TermPartical", resultTerm);
            return View("IndexTerm", resultTerm);
        }

        async public Task<ActionResult> IndexTerms()
        {
            var termsColection = await db.Terms.ToListAsync();
            Mapper.Initialize(cfg => cfg.CreateMap<Term, PreviewTermViewModel>()
                .ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault().Description))
                .ForMember("URL", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault().URL)));
            var resultTermsColection = Mapper.Map<IEnumerable<Term>, IEnumerable<PreviewTermViewModel>>(termsColection);
            if (Request.IsAjaxRequest())
                return PartialView("PreviewTermsPartical", resultTermsColection);
            return View("IndexPreviewTerms", resultTermsColection);
        }
    }
}