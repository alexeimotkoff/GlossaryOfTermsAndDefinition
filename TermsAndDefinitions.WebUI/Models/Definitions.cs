namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Definitions
    {
        [Key]
        public int IdDefinition { get; set; }

        public int IdTerm { get; set; }

        public int? Frequency { get; set; }

        [Required]
        [StringLength(1)]
        [FullTextIndex]
        public string Description { get; set; }

        [Required]
        [StringLength(1)]
        public string URL { get; set; }

        public virtual Terms Terms { get; set; }
    }
}
