﻿using System;
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
        List<string> querySearch = new List<string>();


        public SearchQuery() {
            CountSearchItem = 10;
            TypeSearching = -1;
        }

        public SearchQuery(List<string> querys, int type = -1, int count = 10)
        {
            CountSearchItem = count;
            TypeSearching = type;
            QueryToList = querys;
            QueryString = "";
            if (querys != null )
                if (querys.Count > 0)
                    QueryString = string.Join("/", querys);
        }

        public SearchQuery(string query, int type = -1, int count = 10)
        {
            CountSearchItem = count;
            TypeSearching = type;
            QueryString = query;
            if(!string.IsNullOrEmpty(query))
            QueryToList = query.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        
        public string QueryString { get; set;}

        public bool FirtstIsChar
        {
            get
            {
              if (!string.IsNullOrEmpty(FirstQuery))
                        return rx.Match(querySearch[0]).Success && querySearch[0].Length == 1;
                return false;
            }
        }

        public string FirstQuery
        {
            get {
                if (querySearch.Count > 0)
                    return querySearch[0];
                return "";
            }
        }
      
        public List<string> QueryToList
        {
            get
            {
                return querySearch;
            }
            set
            {
                querySearch.Clear();
                querySearch.AddRange(value.Select(query => query.ToLower()).ToList());
            }
        }

        public  int CountSearchItem {get; set;}

        public int TypeSearching { get; set; }
    }
    

}