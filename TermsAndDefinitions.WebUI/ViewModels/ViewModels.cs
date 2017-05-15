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
        string TermName { get; set; }
        VDefinition Definition { get; set; }
    }

    public class VDefinition
    {
        public VDefinition(string description, string url)
        {
            Description = description;
            URL = url;
        }
        string Description { get; set; }
        string URL { get; set; }
    }

    public class VGlossary
    {
        public VGlossary()
        {

        }
        List<VTerm> terms { get; set; }

    }


}