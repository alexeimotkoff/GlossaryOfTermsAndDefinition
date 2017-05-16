using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TermsAndDefinitions.WebUI.ViewModels
{
    public class SearchQuery
    {
        public SearchQuery() {
            countSearchItem = 10;
            querySearch = "";
            typeSearching = -1;
        }

        public  SearchQuery(string query = "", int  count=10, int type = -1)
        {
            countSearchItem = count;
            querySearch = query;
            typeSearching = type;
        }

     
      public string querySearch { get; set; }
      public  int countSearchItem {get; set;}
      public int typeSearching { get; set; }
    }
}