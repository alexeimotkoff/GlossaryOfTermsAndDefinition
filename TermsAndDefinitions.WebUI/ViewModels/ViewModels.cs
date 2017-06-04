using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TermsAndDefinitions.WebUI.Models;

namespace TermsAndDefinitions.WebUI.ViewModels
{
    public class TermViewModel
    {
        public string TermName { get; set; }
        public IEnumerable<string> Definitions { get; set; }
        public IEnumerable<string> URLs { get; set; }
    }

    public class PreviewTermViewModel
    {
        public string TermName { get; set; }
        public string Definition { get; set; }
        public string URL { get; set; }
    }

    //public class VDefinition
    //{
    //    public VDefinition() { }
    //    public VDefinition(string description, string url)
    //    {
    //        Description = description;
    //        URL = url;
    //    }

    //    public VDefinition( Definition  definition)
    //    {
    //        Description = definition.Description;
    //        URL = definition.URL;
    //        Term = definition.Term.TermName;
    //    }
    //    public string Term { get; set; }
    //   public string Description { get; set; }
    //   public string URL { get; set; }
    //}

    //public class VProject
    //{
    //    public VProject() { }

    //    public VProject(Project project)
    //    {
    //        Glossary = new List<VTerm>();
    //        ProjectName = project.ProjectName;
    //        InformationSystems = project.InformationSystem?.NameInformationSystem;
    //        LifeCycle = project.LifeСycle?.NameLifeСycle;
    //        DocumetationURLs = project.References.Select(x => x.URLToFile).ToList();
    //        foreach (var term in project.Terms)
    //            Glossary.Add(new VTerm(term));
    //    }        

        //public string Annotation {get; set;}

        //public string ProjectName { get; set; }

        //public string InformationSystems { get; set; }

        //public string LifeCycle { get; set; }

        //public List<string> DocumetationURLs = new List<string>();

        //public List<VTerm> Glossary {get; set;}
    //}
    
}