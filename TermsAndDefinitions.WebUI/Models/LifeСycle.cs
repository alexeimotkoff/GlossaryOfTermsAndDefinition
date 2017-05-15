namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LifeСycle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LifeСycle()
        {
            Projects = new HashSet<Project>();
        }

        [Key]
        public int IdLifeСycle { get; set; }

        [Required]
        [StringLength(1)]
        public string NameLifeСycle { get; set; }

        [Required]
        [StringLength(1)]
        public string DescriptonLifeСycle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
