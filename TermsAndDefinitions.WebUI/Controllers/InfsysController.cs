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
    public class InfsysController : Controller
    {
        // GET: Infsys
        DBContext db = new DBContext();
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(int? id)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Project, PreviewProjectViewModel>()
                .ForMember("ProjectName", opt => opt.MapFrom(c => c.ProjectName))
                .ForMember("Annotation", opt => opt.MapFrom(c => c.Annotation));
                cfg.CreateMap<InformationSystem, InfSysViewModel>()
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem))
                .ForMember("Discription", opt => opt.MapFrom(c => c.DescriptonInformationSystem));
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
            });

            if (id == null)
            {
                var infsysColection = new DBContext().InformationSystems.OrderBy(t => t.NameInformationSystem);
                var resultColection = Mapper.Map<IEnumerable<InformationSystem>, IEnumerable<InfSysViewModel>>(infsysColection);

                if (Request.IsAjaxRequest())
                    return PartialView("PreviewInfsysPartical", resultColection);

                return View("IndexPreviewInfsys", resultColection);
            }
            else
            {
                var infsysColection = db.InformationSystems.Find(id);
                var resultColection = Mapper.Map<InformationSystem, InfSysViewModel>(infsysColection);
                if (Request.IsAjaxRequest())
                    return PartialView("InfsysPartical", resultColection);
                return View("IndexInfsys", resultColection);
            }
        }

        public ActionResult PreviewInfsysPartical()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Project, ProjectViewModel>();
                cfg.CreateMap<InformationSystem, InfSysViewModel>()
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem))
                .ForMember("Discription", opt => opt.MapFrom(c => c.NameInformationSystem));
            });
            ViewData["anotherTitle"] = "Информационные системы";
            var infsysColection = new DBContext().InformationSystems.OrderBy(t => t.NameInformationSystem);
            var resultColection = Mapper.Map<IEnumerable<InformationSystem>, IEnumerable<InfSysViewModel>>(infsysColection);

            return PartialView("PreviewInfsysPartical", resultColection);
        }
    }
}