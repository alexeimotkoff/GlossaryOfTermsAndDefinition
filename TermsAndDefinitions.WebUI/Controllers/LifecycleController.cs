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
    public class LifecycleController : Controller
    {
        // GET: Lifecycle
        DBContext db = new DBContext();
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(int? id)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Project, PreviewProjectViewModel>()
                .ForMember("ProjectName", opt => opt.MapFrom(c => c.ProjectName))
                .ForMember("Annotation", opt => opt.MapFrom(c => c.Annotation));
                cfg.CreateMap<LifeCycle, LifeСycleViewModel>()
                .ForMember("NameLifeСycle", opt => opt.MapFrom(c => c.NameLifeСycle))
                .ForMember("DescriptonLifeСycle", opt => opt.MapFrom(c => c.DescriptonLifeСycle));
                cfg.CreateMap<LifeCycle, PreviewLifeСycle>()
                .ForMember("NameLifeСycle", opt => opt.MapFrom(c => c.NameLifeСycle));
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
            });

            if (id == null)
            {
                var lifecycColection = new DBContext().LifeCycles.OrderBy(t => t.NameLifeСycle);
                var resultColection = Mapper.Map<IEnumerable<LifeCycle>, IEnumerable<LifeСycleViewModel>>(lifecycColection);

                if (Request.IsAjaxRequest())
                    return PartialView("PreviewLifecycPartical", resultColection);

                return View("IndexPreviewLifecyc", resultColection);
            }
            else
            {
                var lifecycColection = db.LifeCycles.Find(id);
                var resultColection = Mapper.Map<LifeCycle, LifeСycleViewModel>(lifecycColection);
                if (Request.IsAjaxRequest())
                    return PartialView("LifecycPartical", resultColection);
                return View("IndexLifecyc", resultColection);
            }
        }

        public ActionResult PreviewLifecycPartical()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Project, ProjectViewModel>();
                cfg.CreateMap<LifeCycle, LifeСycleViewModel>()
                .ForMember("NameLifeСycle", opt => opt.MapFrom(c => c.NameLifeСycle))
                .ForMember("DescriptonLifeСycle", opt => opt.MapFrom(c => c.DescriptonLifeСycle));
            });
            ViewData["anotherTitle"] = "Жизненные циклы";
            var lifecycColection = new DBContext().LifeCycles.OrderBy(t => t.NameLifeСycle);
            var resultColection = Mapper.Map<IEnumerable<LifeCycle>, IEnumerable<LifeСycleViewModel>>(lifecycColection);

            return PartialView("PreviewLifecycPartical", resultColection);
        }
    }
    }