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
        
        public SearchQuery() : this("", 10, -1) { }

        public SearchQuery(string query, int type) : this(query, 0, type) {  }

        public  SearchQuery(string query, int  count, int type)
        {
            countSearchItem = count;
            queryString = query.ToLower();
            typeSearching = type;
        }

      public bool IsChar
        {
            get
            {
                if (string.IsNullOrEmpty(queryString))
                    return false;
                else
                    return rx.Match(queryString).Success && queryString.Length == 1;
            }
        }
      public string queryString { get; set; }
      public  int countSearchItem {get; set;}
      public int typeSearching { get; set; }
    }
}