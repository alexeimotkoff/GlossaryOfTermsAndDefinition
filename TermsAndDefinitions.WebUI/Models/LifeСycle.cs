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
    
    public partial class LifeСycle
    {
        public LifeСycle()
        {
            this.Projects = new HashSet<Project>();
        }
    
        public int IdLifeСycle { get; set; }
        public string NameLifeСycle { get; set; }
        public string DescriptonLifeСycle { get; set; }
    
        public virtual ICollection<Project> Projects { get; set; }
    }
}
