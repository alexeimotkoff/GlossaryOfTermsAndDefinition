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
    
    public partial class InformationSystem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InformationSystem()
        {
            this.Terms = new HashSet<Term>();
        }
    
        public int IdInformationSystem { get; set; }
        public string NameInformationSystem { get; set; }
        public string DescriptonInformationSystem { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Term> Terms { get; set; }
    }
}
