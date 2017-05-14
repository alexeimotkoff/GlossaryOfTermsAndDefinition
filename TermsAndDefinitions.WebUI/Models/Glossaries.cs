namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Glossaries
    {
        [Key]
        public int IdGlossary { get; set; }

        public int IdTerm { get; set; }

        public int IdProject { get; set; }

        public virtual Terms Terms { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
