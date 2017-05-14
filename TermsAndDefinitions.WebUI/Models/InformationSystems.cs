namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InformationSystems
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InformationSystems()
        {
            Terms = new HashSet<Terms>();
        }

        [Key]
        public int IdInformationSystem { get; set; }

        [Required]
        [StringLength(1)]
        public string NameInformationSystem { get; set; }

        [Required]
        [StringLength(1)]
        public string DescriptonInformationSystem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Terms> Terms { get; set; }
    }
}
