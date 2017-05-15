using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TermsAndDefinitions.WebUI.ViewModel
{
    public class VTerm
    {
        public VTerm(string termName)
        {
            TermName = termName;
        }
        public string TermName { get; set; }
        public VDefinition Definition { get; set; }
    }

    public class VDefinition
    {
        public VDefinition(string description, string url)
        {
            Description = description;
            URL = url;
        }
       public string Description { get; set; }
       public string URL { get; set; }
    }

    public class VGlossary
    {
        public VGlossary()
        {

        }
        public List<VTerm> terms { get; set; }

    }


}