using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermsAndDefinitions.Domain.Stemmers
{
    public interface IStemmer
    {
        string Stem(string s);
    }
}
