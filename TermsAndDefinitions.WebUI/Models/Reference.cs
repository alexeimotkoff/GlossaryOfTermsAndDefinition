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
    
    public partial class Reference
    {
        public int IdProject { get; set; }
        public string URLToFile { get; set; }
    
        public virtual Project Project { get; set; }
    }
}
