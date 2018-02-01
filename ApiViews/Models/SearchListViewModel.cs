using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mejor_precio_2.Models;
using Microsoft.AspNetCore.Http;

namespace ApiViews.Models
{
    public class SearchListViewModel
    {
        private List<Search> searchList;
        public List<Search> SearchList
        {
            get { return searchList;}
            set { searchList = value;}
        }

        public IFormFile BarcodeImage { get; set; }

        public string BarcodeString { get; set; }
    }
}