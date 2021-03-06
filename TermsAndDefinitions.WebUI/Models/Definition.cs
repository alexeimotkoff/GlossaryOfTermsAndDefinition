//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Definition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Definition()
        {
            this.Projects = new HashSet<Project>();
        }
    
        public int IdDefinition { get; set; }
        public int IdTerm { get; set; }
        public string Description { get; set; }
        public string DescriptionEng { get; set; }
        public string URL { get; set; }
        public string URLTitle { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
    
        public virtual Term Term { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
