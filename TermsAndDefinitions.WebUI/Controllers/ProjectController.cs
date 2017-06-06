using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain = TermsAndDefinitions.Domain;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;
using DBContext = TermsAndDefinitions.WebUI.Models.GlossaryProjectDatabaseEntities;
using AutoMapper;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.InteropServices.ComTypes;
using DotNetOpenAuth.Messaging;

namespace TermsAndDefinitions.WebUI.Controllers
{

    public class ProjectController : Controller
    {
        //
        // GET: /Project/
        //
        Domain.MinHash minHash = new Domain.MinHash();
        Random rnd = new Random(1337);

        DBContext db = new DBContext();

        async public Task<ActionResult> IndexProjects()
        {
            var projects = await db.Projects.ToListAsync();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
                cfg.CreateMap<Project, PreviewProjectViewModel>();
            });
            var resultProjectsColection = Mapper.Map<IEnumerable<Project>, IEnumerable<PreviewProjectViewModel>>(projects);
            if (Request.IsAjaxRequest())
                return PartialView("PreviewProjectsPartical", resultProjectsColection);
            return View("IndexPreviewProjects", resultProjectsColection);
        }

        async public Task<ActionResult> IndexProject(string name)
        {
            var projects = await db.Projects.FirstOrDefaultAsync(p => p.ProjectName == name);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
                cfg.CreateMap<Definition, DefinitionViewModel>();
                cfg.CreateMap<Term, PreviewTermViewModel>().ForMember("Definition", opt => opt.MapFrom(c => c.Definitions.OrderByDescending(d => d.Frequency).FirstOrDefault()));
                cfg.CreateMap<Project, ProjectViewModel>().ForMember("Glossary", opt => opt.MapFrom(c => c.Terms));
            });

            var resultProject = Mapper.Map<Project, ProjectViewModel>(projects);
            if (Request.IsAjaxRequest())
                return PartialView("ProjectPartical", resultProject);
            return View("IndexProject", resultProject);
        }
        [ChildActionOnly]
        public ActionResult PreviewProjectsPartical(IEnumerable<PreviewProjectViewModel> id)
        {
            ViewData["anotherTitle"] = "Встречается в проектах";
            return PartialView("PreviewProjectsPartical", id);
        }
        [ChildActionOnly]
        public ActionResult ProjectPartical(ProjectViewModel id)
        {
            return PartialView("ProjectPartical", id);
        }
        

        async public Task<IEnumerable<Project>> GetSimilarProgects(int[] signature, List<long> buckets, int count)
        {
            HashSet<Project> projects = new HashSet<Project>();
            foreach (long bucketHash in buckets)
            {
                var bucket = await db.BucketHashes.FirstOrDefaultAsync(b => b.Hash == bucketHash);
                if (bucket != null)
                    projects.AddRange(bucket.Projects);
            }
            var result = projects.Select((x) =>
            {
                double similarity_value = minHash.Similarity(signature, x.MinHashes.Select(m=> m.MinHash1).ToArray());
                return new Tuple<Project, double>(x, similarity_value);
            });
            
            if (count < 0 || count > result.Count()) count = result.Count();
            return result.OrderByDescending(x => x.Item2).Take(count).Select(x => x.Item1);            
        }

        [HttpPost]
        public ActionResult AddDocumentation(IEnumerable<HttpPostedFileBase> uploads)
        {
            //new InformationSystem().NameInformationSystem.DescriptonInformationSystem.IdInformationSystem

            foreach (var file in uploads)
            {
                if (file != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    // сохраняем файл в папку Files в проекте
                    file.SaveAs(Server.MapPath("~/Files/" + fileName));
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddProject()
        {

            var informationSysList = db.InformationSystems;
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
            });

            ProjectViewModel project = new ProjectViewModel()
            {
                InfSysList = Mapper.Map<IEnumerable<InformationSystem>, IEnumerable<PreviewInfSysViewModel>>(informationSysList)
            };

            return View(project);
        }

        [HttpPost]
        async public Task<ActionResult> AddProject(ProjectViewModel project)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PreviewInfSysViewModel, InformationSystem>()
                .ForMember("IdInformationSystem", opt => opt.MapFrom(c => c.Id));              
                cfg.CreateMap<ProjectViewModel, Project>();
            });

            string text = getTextFromDoc(project.File);
            string annotation = 
            int[] signature = minHash.GetSignature(text);
            List<long> buckets = minHash.GetBuckets(signature).ToList();
            var similarProgects = GetSimilarProgects(signature, buckets, 4);            
            await db.SaveChangesAsync();
            var project_signature = signature.Select(x =>
             {
               var minHas = db.MinHashes.FirstOrDefault( m => m.MinHash1 == x);
               if (minHas == null)
               {
                   minHas = new MinHash() { IdMinHash = -1, MinHash1 = x };
               }
               return minHas;
           }
           );
            db.MinHashes.AddRange(project_signature.Where(s => s.IdMinHash == -1));
            await db.SaveChangesAsync();
            var project_buckets = buckets.Select(x =>
            {
                var bucket = db.BucketHashes.FirstOrDefault(m => m.Hash == x);
                if (bucket == null)
                {
                    bucket = new BucketHash() { IdHash = -1, Hash = x };
                }
                return bucket;
            }
           );

            db.BucketHashes.AddRange(project_buckets.Where(s => s.IdHash == -1));
            await db.SaveChangesAsync();

            Project new_project = Mapper.Map<ProjectViewModel, Project>(project);

            foreach (var bucket in project_buckets)
            {
                new_project.BucketHashes.Add(bucket);
            }
            foreach (var minHash in project_signature)
            {
                new_project.MinHashes.Add(minHash);
            }
            db.Projects.Add(new_project);
            await db.SaveChangesAsync();
            

            return View();
        }

        public string getTextFromDoc(HttpPostedFileBase doc)
        {
            return "";
        }

        public string getAnotationFromText(string text)
        {
            return "";
        }        
    }
}
