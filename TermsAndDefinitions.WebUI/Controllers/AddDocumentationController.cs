using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain = TermsAndDefinitions.Domain;
using TermsAndDefinitions.WebUI.Models;
using TermsAndDefinitions.WebUI.ViewModels;

namespace TermsAndDefinitions.WebUI.Controllers
{

    public class AddDocumentationController : Controller
    {
        //
        // GET: /AddDocumentation/
        //
        Domain.MinHash minHash = new Domain.MinHash();
        Random rnd = new Random(1337);
        int countMinHash = 100;
        GlossaryProjectDatabaseEntities db = new GlossaryProjectDatabaseEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowProjectsGlossaries(VProject projectFiles )
        {
            List<Project> similarProgects = 
            return View();
        }

        public IEnumerable<Project> GetSimilarProgects(string text, int count)
        {
            int[] signature = minHash.GetSignature(text);
            List<long> buckets = minHash.GetBuckets(signature).ToList();
            var bucket = db.BucketHashes.Where((x) => buckets.Contains(x.Hash));
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
        public ActionResult AddDocumetation(Project project)
        {

            return View();
        }

        [HttpPost]
        public bool AddGlossary(VProject glossary)
        {
            foreach (var pair in glossary)
            {
                Term term = db.Terms.FirstOrDefault(x => x.TermName.ToLower() == x.TermName.ToLower().Trim());
                if (term == null)
                {
                    term = pair.Key;
                    db.Terms.Add(term);
                    db.SaveChanges();
                }
                Definition definition = new Definition() { Description = description, URL = url, IdTerm = term.IdTerm };

            }


        }


        //public ActionResult AddTerm(string nameTerm, string description, string url)
        //{

        //}

    }
}
