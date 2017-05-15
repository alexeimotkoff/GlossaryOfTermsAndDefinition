using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TermsAndDefinitions.WebUI.ViewModels
{
    public class SearchQuery
    {
      public  SearchQuery( int? type, string query = "", int  count=10)
        {
            countSearchItem = count;
            querySearch = query;
            typeSearching = type;
        }

     
      public string querySearch { get; set; }
      public  int countSearchItem {get; set;}
       public int? typeSearching { get; set; }
    }
}