using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class PriceInfoViewModel : PriceInfo
    {
        public PriceInfoViewModel(PriceInfo priceInfo){
            this.Id = priceInfo.Id;
            this.Product = priceInfo.Product;
            this.Store = priceInfo.Store;
            this.Cost = priceInfo.Cost;
            this.Report = priceInfo.Report;
            this.Date = priceInfo.Date;
        }

        public PriceInfoViewModel(){}
    }
}