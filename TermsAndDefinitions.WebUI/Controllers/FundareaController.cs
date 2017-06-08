using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;
using DBContext = TermsAndDefinitions.WebUI.Models.GlossaryProjectDatabaseEntities;

namespace TermsAndDefinitions.WebUI.Controllers
{
    public class FundareaController : Controller
    {
        // GET: Fundarea
        DBContext db = new DBContext();
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var fundareaColection = new DBContext().FundamentalAreas.OrderBy(t => t.NameFundamentalArea);
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<FundamentalArea, PreviewFundArea>();
                    //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
                    cfg.CreateMap<Definition, DefinitionViewModel>();
                    cfg.CreateMap<Term, PreviewTermViewModel>().ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault()));
                });
                var resultColection = Mapper.Map<IEnumerable<FundamentalArea>, IEnumerable<PreviewFundArea>>(fundareaColection);

                if (Request.IsAjaxRequest())
                    return PartialView("PreviewFundareaPartical", resultColection);

                return View("IndexPreviewFundarea", resultColection);
            }
            else
            {
                var fundareaColection = db.FundamentalAreas.Find(id);
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<FundamentalArea, PreviewFundArea>();
                    //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
                    cfg.CreateMap<Definition, DefinitionViewModel>();
                    cfg.CreateMap<Term, PreviewTermViewModel>().ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault()));
                });

                var resultColection = Mapper.Map<FundamentalArea, PreviewFundArea>(fundareaColection);
                if (Request.IsAjaxRequest())
                    return PartialView("PreviewFundareaPartical", resultColection);
                return View("IndexPreviewFundarea", resultColection);
            }
        }
    }
}