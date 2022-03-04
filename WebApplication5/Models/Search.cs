using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Search
    {
        public string search { get; set; }
        public Search()
        {
            search = string.Empty;
        }
        public Search(string _search)
        {
            search=_search;
        }
    }
}