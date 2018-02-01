using System;
using System.Collections.Generic;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class PriceDataViewModel
    {
        public List<ProductInfo> ProductList { get; set; }

        public List<Store> StoreList { get; set; }
        
        public Guid Id { get; set; }

        public Guid IdProduct { get; set; }

        public Guid IdStore { get; set; }

        public decimal Cost { get; set; }

        public string Report { get; set; }
    }
}