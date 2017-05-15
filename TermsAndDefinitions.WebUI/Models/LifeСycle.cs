namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LifeСycle
    {
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
<<<<<<< HEAD

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
=======
    
>>>>>>> parent of 9ed99d5... Перенесена база данных на Microsoft Azure. Создана модель на основе данной базы данных.
        public virtual ICollection<Project> Projects { get; set; }
    }
}
