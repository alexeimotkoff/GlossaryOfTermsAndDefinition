using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TermsAndDefinitions.WebUI.Models;

namespace TermsAndDefinitions.WebUI.ViewModels
{
    public class VTerm
    {
        public VTerm(string termName)
        {
            TermName = termName;
        }

        public VTerm(Term term)
        {
            TermName = term.TermName;
            Definition discription = term.Definitions.OrderByDescending(y => y.Frequency).FirstOrDefault();
            Description = new VDefinition(discription);
        }

        public string TermName { get; set; }
        public VDefinition Description { get; set; }
    }

    public class VDefinition
    {
        public VDefinition(string description, string url)
        {
            Description = description;
            URL = url;
        }

        public VDefinition( Definition  definition)
        {
            Description = definition.Description;
            URL = definition.URL;
        }
       public string Description { get; set; }
       public string URL { get; set; }
    }

    public class VGlossary
    {
        public VGlossary(Project project)
        {
            terms = project.Terms.OrderBy(x => x.TermName).Select(x => new VTerm(x)).ToList();
        }
        public List<VTerm> terms { get; set; }

    }


}