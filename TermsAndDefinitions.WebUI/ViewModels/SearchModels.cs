using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TermsAndDefinitions.WebUI.ViewModels
{
    public class SearchQuery
    {
        Regex rx = new Regex("[а-яёa-z]");
        
        public SearchQuery() {
            countSearchItem = 10;
            querySearch = "";
            typeSearching = -1;
        }

        public  SearchQuery(string query = "", int  count=10, int type = -1)
        {
            countSearchItem = count;
            querySearch = query.ToLower();
            typeSearching = type;
        }

      public bool IsChar
        {
            get
            {
                return rx.Match(querySearch).Success && querySearch.Length == 1;
            }
        }
      public string querySearch { get; set; }
      public  int countSearchItem {get; set;}
      public int typeSearching { get; set; }
    }
}