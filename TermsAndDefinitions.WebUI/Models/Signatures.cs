namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Signatures
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdSignature { get; set; }

        public int IdProject { get; set; }

        [Required]
        [StringLength(1)]
        public string Hash1 { get; set; }

        [Required]
        [StringLength(1)]
        public string HashN { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
