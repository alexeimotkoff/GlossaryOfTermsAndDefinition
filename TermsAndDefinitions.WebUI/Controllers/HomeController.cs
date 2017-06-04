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
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        //
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            //List<VTerm> terms = new List<VTerm>();
            //foreach (var term in db.Terms)
            //    terms.Add(new VTerm(term));            
           return View();
        }

        //async public Task<IEnumerable<VPreviewTerm>> Terms()
        //{
        //    var termsColection = await db.Terms.ToListAsync();
        //    return termsColection;
        //}
        async public Task<ActionResult> Term(string name)
        {
            var term = await db.Terms.FirstOrDefaultAsync(t => t.TermName == name);
            Mapper.Initialize(cfg => cfg.CreateMap<Term, TermViewModel>()
                .ForMember("Definitions", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).Select(x => x.Description)))
                .ForMember("URLs", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).Select(x => x.URL))));
            var resultTerm = Mapper.Map<Term, TermViewModel>(term);
            return PartialView("Term", resultTerm);
        }

        async public Task<ActionResult> Terms()
        {
            var termsColection = await db.Terms.ToListAsync();
            Mapper.Initialize(cfg => cfg.CreateMap<Term, PreviewTermViewModel>()
                .ForMember("Definition",  opt => opt.MapFrom( c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault().Description))
                .ForMember("URL",  opt => opt.MapFrom( c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault().URL)));
            var resultTermsColection = Mapper.Map<IEnumerable<Term>, IEnumerable<PreviewTermViewModel>>(termsColection);
            return PartialView("PreviewTerms", resultTermsColection);
        }
     
    }
}
