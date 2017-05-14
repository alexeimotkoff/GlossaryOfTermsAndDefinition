namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Projects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Projects()
        {
            Glossaries = new HashSet<Glossaries>();
            Signatures = new HashSet<Signatures>();
        }

        [Key]
        public int IdProject { get; set; }

        [Required]
        [StringLength(1)]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(1)]
        public string ReferenceToProject { get; set; }

        [Required]
        [StringLength(1)]
        public string ReferenceToFiles { get; set; }

        [Required]
        [StringLength(1)]
        public string Signature { get; set; }

        [Required]
        [StringLength(1)]
        public string Annotation { get; set; }

        public int IdLifeСycle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Glossaries> Glossaries { get; set; }

        public virtual LifeСycle LifeСycle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Signatures> Signatures { get; set; }
    }
}
