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
    
    public partial class Project
    {
        public Project()
        {
            this.Signatures = new HashSet<Signature>();
            this.Terms = new HashSet<Term>();
        }
    
        public int IdProject { get; set; }
        public string ProjectName { get; set; }
        public string ReferenceToProject { get; set; }
        public string ReferenceToFiles { get; set; }
        public string Signature { get; set; }
        public string Annotation { get; set; }
        public Nullable<int> IdLifeСycle { get; set; }
    
        public virtual LifeСycle LifeСycle { get; set; }
        public virtual ICollection<Signature> Signatures { get; set; }
        public virtual ICollection<Term> Terms { get; set; }
    }
}
