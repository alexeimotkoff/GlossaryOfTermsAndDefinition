using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;
using DBContext = TermsAndDefinitions.WebUI.Models.GlossaryProjectDatabaseEntities;
using AutoMapper;
using System.Threading.Tasks;
using System.Data.Entity;
using DotNetOpenAuth.Messaging;
using System.IO;
using TikaOnDotNet.TextExtraction;
using System.Web.UI;
using System.Text.RegularExpressions;

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
        List<Project> Projects = new DBContext().Projects.ToList();

        [OutputCache(Duration = 300, Location = OutputCacheLocation.Any)]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var projects = Projects;
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<LifeCycle, PreviewLifeСycle>();
                    cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                    .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                    .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                    cfg.CreateMap<Project, PreviewProjectViewModel>();
                });
                var resultProjectsColection = Mapper.Map<IEnumerable<Project>, IEnumerable<PreviewProjectViewModel>>(projects);
                if (Request.IsAjaxRequest())
                    return PartialView("PreviewProjectsPartical", resultProjectsColection);
                return View("IndexPreviewProjects", resultProjectsColection);
            }
            else
            {
                var projects = Projects.FirstOrDefault(x => x.IdProject == id);
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<LifeCycle, PreviewLifeСycle>();
                    cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                    .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                    .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                    cfg.CreateMap<Definition, DefinitionViewModel>()
                    .ForMember("TermName", opt => opt.MapFrom(c => c.Term.TermName));
                    cfg.CreateMap<Project, ProjectViewModel>();
                });
                var resultProject = Mapper.Map<Project, ProjectViewModel>(projects);
                if (Request.IsAjaxRequest())
                    return PartialView("ProjectPartical", resultProject);
                return View("IndexProject", resultProject);
            }
        }
        

        [HttpGet]
        public ActionResult PreviewProjectsPartical(IEnumerable<PreviewProjectViewModel> project)
        {
            ViewData["anotherTitle"] = "Встречается в проектах";
            return PartialView("PreviewProjectsPartical", project);
        }


        [HttpGet]
        public ActionResult ProjectPartical(ProjectViewModel project)
        {
            return PartialView("ProjectPartical", project);
        }

        async public Task<IEnumerable<Project>> GetSimilarProgects(int idProject, int count)
        {
           var project = db.Projects.Find(idProject);
           int[] signature = project.MinHashes.Select(x => x.MinHash1).ToArray();
           List<long> buckets = project.BucketHashes.Select(x => x.Hash).ToList();
           return await GetSimilarProgects(signature,  buckets,  count);
        }
        
        async public Task<IEnumerable<Project>> GetSimilarProgects(int[] signature, List<long> buckets, int count)
        {         
            HashSet <Project> projects = new HashSet<Project>();
            foreach (long bucketHash in buckets)
            {
                var bucket = await db.BucketHashes.FirstOrDefaultAsync(b => b.Hash == bucketHash);
                if (bucket != null)
                    projects.AddRange(bucket.Projects);
            }
            var result = projects.Select((x) =>
            {
                double similarity_value = minHash.Similarity(signature, x.MinHashes.Select(m => m.MinHash1).ToArray());
                return new Tuple<Project, double>(x, similarity_value);
            });

            if (count < 0 || count > result.Count()) count = result.Count();
            return result.OrderByDescending(x => x.Item2).Take(count).Select(x => x.Item1);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var informationSysList = db.InformationSystems;
            var lifeСycleList = db.LifeCycles;
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LifeCycle, PreviewLifeСycle>();
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
            });

            ProjectViewModel project = new ProjectViewModel()
            {
                LifeСycleList = Mapper.Map<IEnumerable<LifeCycle>, IEnumerable<PreviewLifeСycle>>(lifeСycleList),
                InfSysList = Mapper.Map<IEnumerable<InformationSystem>, IEnumerable<PreviewInfSysViewModel>>(informationSysList)
            };
            if (Request.IsAjaxRequest())
                return PartialView("AddPartical", project);
            return View("IndexAdd", project);
        }

        [HttpPost, ActionName("Add")]
        //[ValidateAntiForgeryToken]
        public ActionResult AddPost(ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PreviewLifeСycle,LifeCycle>();
                    cfg.CreateMap<PreviewInfSysViewModel, InformationSystem>()
                    .ForMember("IdInformationSystem", opt => opt.MapFrom(c => c.Id));
                    cfg.CreateMap<ProjectViewModel, Project>()
                    .ForMember("References", opt => opt.MapFrom(c => Server.MapPath(string.Format("~/Files/{0}/",c.ProjectName.Replace(' ', '_')))));
            });

                string pathToProectFolder = SaveDocuments(project.File, project.ProjectName);
                var texts = GetTextsFromDocs(pathToProectFolder);
                var annotations = GetAnnotationFromTexts(texts);
                string annotation = "";
                string textForSignature = project.ProjectName + texts.Aggregate((x, y) => x + y);

                if (annotations.Count > 0)
                {
                    annotation = annotations.First();
                    textForSignature = project.ProjectName + annotation;
                }

                int[] signature = minHash.GetSignature(textForSignature);
                //var minHashColectionObjectFromDB = db.MinHashes.Where(x => signature.Contains(x.MinHash1));
                //var minHashColectionValuesFromDB = minHashColectionObjectFromDB.Select(x => x.MinHash1);
                var new_project_signature = signature.Select(x => new MinHash() { MinHash1 = x });
                    //.Where(x => !minHashColectionValuesFromDB.Contains(x))
                
                var buckets = minHash.GetBuckets(signature);
                //var bucketHashColectionObjectFromDB = db.BucketHashes.Where(x => buckets.Contains(x.Hash));
                //var bucketHashColectionValuesFromDB = bucketHashColectionObjectFromDB.Select(x => x.Hash);
                var new_project_buckets = buckets.Select(x =>  new BucketHash() { Hash = x });
                    //.Where(x => !bucketHashColectionValuesFromDB.Contains(x))

                db.MinHashes.AddRange(new_project_signature);
                db.BucketHashes.AddRange(new_project_buckets);
                // db.SaveChanges();

                Project new_project = new Project()
                {
                    ProjectName = project.ProjectName,
                    Annotation = annotation,
                    IdLifeСycle = project.LifeСycle.IdLifeСycle,
                    IdInformationSystem = project.InformationSystem.Id
                };

                if (annotations.Count > 0)
                {
                    new_project.Annotation = annotation;
                }

                //foreach (var bucket in bucketHashColectionObjectFromDB)
                //{
                //    new_project.BucketHashes.Add(bucket);
                //}

                foreach (var bucket in new_project_buckets)
                {
                    new_project.BucketHashes.Add(bucket);
                }

                //foreach (var minHash in minHashColectionObjectFromDB)
                //{
                //    new_project.MinHashes.Add(minHash);
                //}

                foreach (var minHash in new_project_signature)
                {
                    new_project.MinHashes.Add(minHash);
                }

                db.Projects.Add(new_project);
                //db.SaveChanges();              
                return RedirectToAction("Edit",new_project.IdProject);
            }
            return View("IndexAdd", project);
        }
        
        [HttpGet]
        [ValidateAntiForgeryToken]
        async public Task<ActionResult> Edit(int id)
        {
            var projects = await db.Projects.FindAsync(id);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InformationSystem, PreviewInfSysViewModel>()
                .ForMember("Id", opt => opt.MapFrom(c => c.IdInformationSystem))
                .ForMember("Name", opt => opt.MapFrom(c => c.NameInformationSystem));
                cfg.CreateMap<Definition, DefinitionViewModel>();                
                cfg.CreateMap<Project, ProjectViewModel>();
            });

            var resultProject = Mapper.Map<Project, ProjectViewModel>(projects);

            return View("IndexEdit", resultProject);
        }

        [HttpPost, ActionName("Edit")]
        async public Task<ActionResult> EditPost(ProjectViewModel project)
        {
            var editProject = await db.Projects.FindAsync(project.IdProject);
            return View("", project);
        }

        public IEnumerable<string> SaveDocuments(List<HttpPostedFileBase> docs, string projectName)
        {
            projectName = projectName.Replace(' ', '_');
            if (docs != null)
            {
                foreach (var doc in docs)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(doc.FileName);
                    // сохраняем файл в папку Files в проекте
                    string path = Server.MapPath(string.Format("~/Files/{0}/{1}", projectName, fileName));
                    doc.SaveAs(path);
                    yield return path;
                }
            }
        }

        public string SaveDocuments(HttpPostedFileBase doc, string projectName)
        {
            projectName = projectName.Replace(' ', '_');
            if (doc != null)
            {
                //получаем имя файла
                string fileName = System.IO.Path.GetFileName(doc.FileName);
                //сохраняем файл в папку Files в проекте
                string path = Server.MapPath(string.Format("~/Files/{0}/", projectName));
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                doc.SaveAs(path + fileName);
                return path;
            }
            return "";
        }

        public List<string> GetTextsFromDocs(string pathToFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(pathToFolder);
            List<string> resultTexts = new List<string>();
            foreach (var doc in dir.GetFiles())
            {
                string text = new TextExtractor().Extract(doc.FullName).Text;
                resultTexts.Add(Regex.Replace(text, @"[\r\n]+", @"\n"));
            }
            return resultTexts;
        }

        public List<string> GetAnnotationFromTexts(IEnumerable<string> texts)
        {
            List<string> annotations = new List<string>();
            foreach (var text in texts)
            {
                string pat = @"аннотация[\s\t.:\n]*([^\n]*)";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Match m = r.Match(text);
                if (m.Success)
                    annotations.Add(m.Groups[1].Value);
            }
            annotations.Sort((x,y) => y.Length.CompareTo(x.Length));
            return annotations;
        }
    }
}
