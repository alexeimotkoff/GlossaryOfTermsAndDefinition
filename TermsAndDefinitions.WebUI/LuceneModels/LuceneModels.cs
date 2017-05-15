using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TermsAndDefinitions.WebUI.Models;
namespace TermsAndDefinitions.WebUI.LucemeModels
{
    public class LTerm
    {
        public LTerm(Term term)
        {
            TermName = term.TermName;
            IdTerm = term.IdTerm;
        }
        string Table { get { return "Term"; } }
        string TermName { get; set; }
        int IdTerm { get; set; }
    }

    public class LDefinition
    {
        String Table { get { return "Definition"; } }
        public LDefinition(Definition definition)
        {
            Description = definition.Description;
            TermName = definition.Term.TermName;
            IdTerm = definition.IdTerm;
            IdDefinition = definition.IdDefinition;
        }
        string Description { get; set; }
        string TermName { get; set; }
        int IdTerm { get; set; }
        int IdDefinition { get; set; }
    }
}