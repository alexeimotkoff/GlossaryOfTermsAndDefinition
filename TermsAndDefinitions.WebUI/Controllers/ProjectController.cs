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


        //public ActionResult ShowProjectsGlossaries(VProject projectFiles )
        //{
        //    //List<Project> similarProgects = 
        //    //return View();
        //}

        async public Task<IEnumerable<Project>> GetSimilarProgects(int[] signature, List<long> buckets, int count)
        {           
            var bucket = await db.BucketHashes.Where((x) => buckets.Contains(x.Hash)).ToListAsync();
            var similarProgects = bucket.SelectMany(b => b.Projects).ToList();
            List<Tuple<Project, double>> result = new List<Tuple<Project, double>>(count);
            foreach (var project in similarProgects)
            {
                double similarity_value = minHash.Similarity(signature, project.MinHashes.Select(x => x.MinHash1).ToArray());

                result.Sort((x, y) => x.Item2.CompareTo(y.Item2));
                if (result.Count < count)
                    new Tuple<Project, double>(project, similarity_value);
                else
                {
                    for (int i = 0; i < result.Count; i++)
                        if (result[i].Item2 < similarity_value)
                            result[i] = new Tuple<Project, double>(project, similarity_value);
                }
            }
            return result.Select(x => x.Item1);
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
                //.ForMember("Descripton", opt => opt.MapFrom(c => c.DescriptonInformationSystem))
                cfg.CreateMap<ProjectViewModel, Project>();
            });

            string text = getTextFromDoc(project.File);
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
            //Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            //object miss = System.Reflection.Missing.Value;
            //object path = @"C:\DOC\myDocument.docx";
            //object readOnly = true;
            //Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            //string totaltext = "";
            //for (int i = 0; i < docs.Paragraphs.Count; i++)
            //{
            //    totaltext += " \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString();
            //}
            //docs.Close();
            //word.Quit();
            //return totaltext;

            return @"1. Мы создаем неуправляемый ресурс, который не соберет сборщик мусора - отдельный процесс в памяти с приложением Word, если мы его не закроем и не выведем на экран, он так и останется там висеть до выключения компьютера. Более того такие ворды могут накапливаться незаметно для пользователя, программист-то еще прибьет их вручную. Заботиться о высвобождения неуправляемого ресурса должен программист. 
                        2.По умолчанию Word запускается невидимым, на экран его выводим мы.Для начала рассмотрим самый простой и примитивный вариант -поиск и замена строки в документе Word. Некоторые программисты так и работают - ставят в шаблон текстовую метку вроде @@nowDate и заменяют ее на нужное значение.

                        Пришло время познакомится с фундаментом работы с Word -великим и ужасным объектом Range. Его суть сложно описать словами - это некоторый произвольный кусок документа, диапазон (range), который может включать в себя все что угодно - от пары символов, до таблиц, закладок и прочих интересных вещей.Не стоит путать его с Selection -куском документа, выделенным мышкой, который само собой можно конвертировать в Range. Соотвественно нам надо получить Range для всего документа, найти нужную строку внутри него, получить Range для этой строки и уже внутри этого последнего диапазона заменить текст на требуемый. И не стоит забывать, что документ может иметь сложную структуру с колонтитулами и прочей ересью, возможный универсальный метод для замены всех вхождений данной строки:";


        }


        //[HttpPost]
        //public bool AddGlossary(VProject glossary)
        //{
        //    foreach (var pair in glossary)
        //    {
        //        Term term = db.Terms.FirstOrDefault(x => x.TermName.ToLower() == x.TermName.ToLower().Trim());
        //        if (term == null)
        //        {
        //            term = pair.Key;
        //            db.Terms.Add(term);
        //            db.SaveChanges();
        //        }
        //        Definition definition = new Definition() { Description = description, URL = url, IdTerm = term.IdTerm };

        //    }


        //}


        //public ActionResult AddTerm(string nameTerm, string description, string url)
        //{

        //}

    }
}
