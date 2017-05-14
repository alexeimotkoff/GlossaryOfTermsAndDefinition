using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermsAndDefinitions.WebUI.Models
{
    /// <summary>
    /// По полю, содержащему данный атрибут, будет построен полнотекстовый индекс
    /// </summary>
    public class FullTextIndexAttribute : Attribute
    {
    }
    public class FullTextIndex
    {
        public enum SearchAlgorithm
        {
            Contains,
            FreeText
        }
    }
}
