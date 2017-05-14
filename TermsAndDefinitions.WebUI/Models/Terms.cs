namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Terms
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Terms()
        {
            Definitions = new HashSet<Definitions>();
            Glossaries = new HashSet<Glossaries>();
        }

        [Key]
        public int IdTerm { get; set; }

        [Required]
        [StringLength(200)]
        public string TermName { get; set; }

        public int IdInformationSystem { get; set; }

        public int IdFundamentalArea { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Definitions> Definitions { get; set; }

        public virtual FundamentalAreas FundamentalAreas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Glossaries> Glossaries { get; set; }

        public virtual InformationSystems InformationSystems { get; set; }
    }
}
