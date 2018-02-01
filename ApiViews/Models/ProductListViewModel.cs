using System;
using System.Collections.Generic;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class ProductListViewModel
    {
        private List<ProductInfo> productList;
        public List<ProductInfo> ProductList
        {
            get { return productList;}
            set { productList = value;}
        }
    }
}