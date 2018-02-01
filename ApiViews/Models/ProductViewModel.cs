using System;
using System.Collections.Generic;
using mejor_precio_2.Models;
using Microsoft.AspNetCore.Http;

namespace ApiViews.Models
{
    public class ProductViewModel : Product{

        public ProductViewModel(Product product){
            this.Id = product.Id;
            this.Name = product.Name;
            this.Barcode = product.Barcode;
        }
        
        public ProductViewModel(){}

        public IFormFile Image { get; set; }
    }
}